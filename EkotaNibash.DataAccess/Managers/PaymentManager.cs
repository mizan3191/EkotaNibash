namespace EkotaNibash.DataAccess
{
    public class PaymentManager : BaseDataManager, IPayment
    {
        public PaymentManager(BoniyadiContext model) : base(model)
        {
        }

        public Task<List<MembershipPayment>> GetAllAsync(int memberId)
            => _dbContext.MembershipPayments
            .Include(x => x.EkotaMember)
            .Where(x => x.EkotaMemberId == memberId)
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        public Task<MembershipPayment> GetByIdAsync(int id) =>
            _dbContext.MembershipPayments
            .FindAsync(id)
            .AsTask();


        public async Task AddAsync(MembershipPayment payment)
        {
            _dbContext.MembershipPayments.Add(payment);
            await _dbContext.SaveChangesAsync();

            // If there's a file attached, create a MemberDocument
            if (payment.File != null && payment.File.Length > 0)
            {
                CreatePaymentDocument(payment);
            }
        }

        public async Task UpdateAsync(MembershipPayment payment)
        {
            _dbContext.MembershipPayments.Update(payment);
            await _dbContext.SaveChangesAsync();
        }

        private void CreatePaymentDocument(MembershipPayment payment)
        {
            var memberDocument = new MemberDocument
            {
                EkotaMemberId = payment.EkotaMemberId,
                MembershipPaymentId = payment.Id,
                DocumentTypeId = 6, // Assuming 6 is "Payment Invoice" from your DocumentTypes
                DocumentName = $"Payment Receipt - {payment.PaymentDate:dd-MMM-yyyy}",
                Description = $"Payment of ৳{payment.Amount:N2} via {payment.PaymentType} on {payment.PaymentDate:dd-MMM-yyyy}",
                FileName = payment.FileName,
                FileType = Path.GetExtension(payment.FileName),
                File = payment.File,
                DocumentDate = payment.PaymentDate
            };

            _dbContext.MemberDocuments.Add(memberDocument);
            _dbContext.SaveChanges();
        }
    }
}
