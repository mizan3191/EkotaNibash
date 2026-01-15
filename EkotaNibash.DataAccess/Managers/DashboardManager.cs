namespace EkotaNibash.DataAccess
{
    public class DashboardManager : BaseDataManager, IDashboard
    {
        private const decimal MONTHLY_FEE = 2000m;
        private static readonly DateTime START_DATE = new DateTime(2025, 6, 1); // June 2025

        public DashboardManager(BoniyadiContext model) : base(model)
        {
        }

        public async Task<DashboardSummary> GetDashboardSummaryAsync()
        {
            var summary = new DashboardSummary();

            // Get current month and year
            var currentDate = DateTime.Now;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            // Total members
            summary.TotalMembers = await _dbContext.EkotaMembers.CountAsync();
            summary.ActiveMembers = await _dbContext.EkotaMembers
                .Where(m => !m.IsInactive)
                .CountAsync();
            summary.InactiveMembers = summary.TotalMembers - summary.ActiveMembers;

            // Total payments
            summary.TotalPayments = await _dbContext.MembershipPayments
                .SumAsync(p => p.Amount);

            // Total expenses
            summary.TotalExpenses = await _dbContext.Expenses
                .Where(e => !e.IsDeleted)
                .SumAsync(e => (decimal)e.Amount);

            // Balance
            summary.Balance = summary.TotalPayments - summary.TotalExpenses;

            // This month's payment
            summary.ThisMonthPayment = await _dbContext.MembershipPayments
                .Where(p => p.PaymentDate.Month == currentMonth &&
                           p.PaymentDate.Year == currentYear)
                .SumAsync(p => p.Amount);

            // This month's expense
            summary.ThisMonthExpense = await _dbContext.Expenses
                .Where(e => e.ExpenseDate.Month == currentMonth &&
                           e.ExpenseDate.Year == currentYear &&
                           !e.IsDeleted)
                .SumAsync(e => (decimal)e.Amount);

            // Payment type summary
            var paymentTypeData = await _dbContext.MembershipPayments
                .GroupBy(p => p.PaymentType)
                .Select(g => new
                {
                    PaymentType = g.Key,
                    TotalAmount = g.Sum(p => p.Amount),
                    Count = g.Count()
                })
                .ToListAsync();

            summary.PaymentTypeSummaries = paymentTypeData.Select(p => new PaymentTypeSummary
            {
                PaymentType = p.PaymentType.ToString(),
                TotalAmount = p.TotalAmount,
                Count = p.Count,
                Percentage = summary.TotalPayments > 0 ?
                    (double)(p.TotalAmount / summary.TotalPayments * 100) : 0
            }).ToList();

            // Expense type summary
            var expenseTypeData = await _dbContext.Expenses
                .Include(e => e.ExpenseType)
                .Where(e => !e.IsDeleted)
                .GroupBy(e => e.ExpenseType.Name)
                .Select(g => new
                {
                    ExpenseType = g.Key,
                    TotalAmount = g.Sum(e => (decimal)e.Amount),
                    Count = g.Count()
                })
                .ToListAsync();

            summary.ExpenseTypeSummaries = expenseTypeData.Select(e => new ExpenseTypeSummary
            {
                ExpenseType = e.ExpenseType,
                TotalAmount = e.TotalAmount,
                Count = e.Count,
                Percentage = summary.TotalExpenses > 0 ?
                    (double)(e.TotalAmount / summary.TotalExpenses * 100) : 0
            }).ToList();

            // Get monthly summary for last 6 months
            summary.MonthlySummaries = await GetMonthlySummaryAsync(6);

            return summary;
        }

        public async Task<List<MonthlySummary>> GetMonthlySummaryAsync(int months = 12)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-months + 1);

            var monthlyData = await _dbContext.MembershipPayments
                .Where(p => p.PaymentDate >= startDate)
                .GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                .Select(g => new MonthlySummary
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthYear = $"{g.Key.Year}-{g.Key.Month:D2}",
                    TotalPayments = g.Sum(p => p.Amount)
                })
                .ToListAsync();

            var monthlyExpenses = await _dbContext.Expenses
                .Where(e => e.ExpenseDate >= startDate && !e.IsDeleted)
                .GroupBy(e => new { e.ExpenseDate.Year, e.ExpenseDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalExpenses = g.Sum(e => (decimal)e.Amount)
                })
                .ToListAsync();

            var monthlyMembers = await _dbContext.EkotaMembers
                .Where(m => m.ApplicationDate >= startDate)
                .GroupBy(m => new { m.ApplicationDate.Year, m.ApplicationDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    NewMembers = g.Count()
                })
                .ToListAsync();

            // Merge data
            foreach (var month in monthlyData)
            {
                var expense = monthlyExpenses.FirstOrDefault(e => e.Year == month.Year && e.Month == month.Month);
                month.TotalExpenses = expense?.TotalExpenses ?? 0;

                var member = monthlyMembers.FirstOrDefault(m => m.Year == month.Year && m.Month == month.Month);
                month.NewMembers = member?.NewMembers ?? 0;
            }

            return monthlyData.OrderBy(m => m.Year).ThenBy(m => m.Month).ToList();
        }

        // Method 1: বাকির তালিকা
        public async Task<List<MemberDueDto>> GetMembersWithDueAsync()
        {
            var members = await _dbContext.EkotaMembers
                .Where(m => !m.IsInactive)
                .Select(m => new
                {
                    Member = m,

                    Payments = m.MembershipPayments,

                    // ✅ LAST uploaded photo
                    LastDoc = _dbContext.MemberDocuments
                        .Where(d =>
                            d.DocumentTypeId == 8 &&
                            d.EkotaMemberId == m.Id
                        )
                        .OrderByDescending(d => d.Id)
                        .Select(d => new
                        {
                            d.File,
                            d.FileName
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = new List<MemberDueDto>();
            var currentDate = DateTime.Now;

            var totalMonthsFromStart = CalculateMonthsBetween(START_DATE, currentDate);

            foreach (var item in members)
            {
                var member = item.Member;

                var expectedMonths = totalMonthsFromStart;

                var totalPaid = item.Payments?
                    .Where(p => (int)p.PaymentType == 1 && !p.IsInactive)
                    .Sum(p => p.Amount) ?? 0;

                var paidMonths = (int)Math.Floor(totalPaid / MONTHLY_FEE);
                var dueMonths = expectedMonths - paidMonths;

                if (dueMonths > 0)
                {
                    var lastPaymentDate = item.Payments?
                        .Where(p => (int)p.PaymentType == 1 && !p.IsInactive)
                        .OrderByDescending(p => p.PaymentDate)
                        .FirstOrDefault()?.PaymentDate;

                    result.Add(new MemberDueDto
                    {
                        MemberId = member.Id,
                        MembershipNo = member.MembershipNo,
                        Name = member.Name,
                        MobileNumber = member.MobileNumber,
                        ApplicationDate = member.ApplicationDate,
                        LastPaymentDate = lastPaymentDate,
                        MonthsDue = dueMonths,
                        DueAmount = dueMonths * MONTHLY_FEE,
                        TotalMonthsMembership = expectedMonths,
                        TotalMonthsPaid = paidMonths,

                        // ✅ PHOTO DATA
                        File = item.LastDoc?.File,
                        FileName = item.LastDoc?.FileName
                    });
                }
            }

            return result
                .OrderByDescending(m => m.MonthsDue)
                .ToList();
        }

        public async Task<List<MemberAdvanceDto>> GetMembersWithAdvanceAsync()
        {
            var members = await _dbContext.EkotaMembers
                .Where(m => !m.IsInactive)
                .Select(m => new
                {
                    Member = m,

                    Payments = m.MembershipPayments,

                    // ✅ LAST uploaded member photo
                    LastDoc = _dbContext.MemberDocuments
                        .Where(d =>
                            d.DocumentTypeId == 8 &&
                            d.EkotaMemberId == m.Id
                        )
                        .OrderByDescending(d => d.Id) // or CreatedDate
                        .Select(d => new
                        {
                            d.File,
                            d.FileName
                        })
                        .FirstOrDefault()
                })
                .ToListAsync();

            var result = new List<MemberAdvanceDto>();
            var currentDate = DateTime.Now;

            var totalMonthsFromStart = CalculateMonthsBetween(START_DATE, currentDate);

            foreach (var item in members)
            {
                var member = item.Member;

                var expectedMonths = totalMonthsFromStart;

                var totalPaid = item.Payments?
                    .Where(p => (int)p.PaymentType == 1 && !p.IsInactive)
                    .Sum(p => p.Amount) ?? 0;

                var expectedAmount = expectedMonths * MONTHLY_FEE;

                if (totalPaid > expectedAmount)
                {
                    var advanceAmount = totalPaid - expectedAmount;
                    var advanceMonths = (int)Math.Floor(advanceAmount / MONTHLY_FEE);

                    var lastPaymentDate = item.Payments?
                        .Where(p => (int)p.PaymentType == 1 && !p.IsInactive)
                        .OrderByDescending(p => p.PaymentDate)
                        .FirstOrDefault()?.PaymentDate;

                    result.Add(new MemberAdvanceDto
                    {
                        MemberId = member.Id,
                        MembershipNo = member.MembershipNo,
                        Name = member.Name,
                        MobileNumber = member.MobileNumber,
                        LastPaymentDate = lastPaymentDate,
                        AdvanceMonths = advanceMonths,
                        AdvanceAmount = advanceAmount,
                        TotalPaid = totalPaid,

                        // ✅ PHOTO DATA
                        File = item.LastDoc?.File,
                        FileName = item.LastDoc?.FileName
                    });
                }
            }

            return result
                .OrderByDescending(m => m.AdvanceMonths)
                .ToList();
        }


        // Helper method to calculate months between two dates (inclusive)
        private int CalculateMonthsBetween(DateTime startDate, DateTime endDate)
        {
            // Ensure start date is at the beginning of the month
            startDate = new DateTime(startDate.Year, startDate.Month, 1);
            // Ensure end date is at the end of the month
            endDate = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month));

            return ((endDate.Year - startDate.Year) * 12) + endDate.Month - startDate.Month + 1;
        }
    }
}
