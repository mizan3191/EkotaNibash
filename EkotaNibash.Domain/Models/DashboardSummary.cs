using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EkotaNibash.Domain.Models
{
    public class DashboardSummary
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal Balance { get; set; }
        public decimal ThisMonthPayment { get; set; }
        public decimal ThisMonthExpense { get; set; }
        public List<MonthlySummary> MonthlySummaries { get; set; } = new();
        public List<PaymentTypeSummary> PaymentTypeSummaries { get; set; } = new();
        public List<ExpenseTypeSummary> ExpenseTypeSummaries { get; set; } = new();
    }

    public class MonthlySummary
    {
        public string MonthYear { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal TotalPayments { get; set; }
        public decimal TotalExpenses { get; set; }
        public int NewMembers { get; set; }
    }

    public class PaymentTypeSummary
    {
        public string PaymentType { get; set; }
        public decimal TotalAmount { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class ExpenseTypeSummary
    {
        public string ExpenseType { get; set; }
        public decimal TotalAmount { get; set; }
        public int Count { get; set; }
        public double Percentage { get; set; }
    }

    public class MemberGrowth
    {
        public string Period { get; set; }
        public int NewMembers { get; set; }
        public int CumulativeTotal { get; set; }
    }

    public class MemberDueDto
    {
        public int MemberId { get; set; }
        public string MembershipNo { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int MonthsDue { get; set; }
        public decimal DueAmount { get; set; }
        public int TotalMonthsMembership { get; set; }
        public int TotalMonthsPaid { get; set; }
    }

    // DTO for Advance Payments (অগ্রিম জমার তালিকা)
    public class MemberAdvanceDto
    {
        public int MemberId { get; set; }
        public string MembershipNo { get; set; }
        public string Name { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? LastPaymentDate { get; set; }
        public int AdvanceMonths { get; set; }
        public decimal AdvanceAmount { get; set; }
        public decimal TotalPaid { get; set; }
    }

    // DTO for Dashboard Summary
    public class DashboardSummaryDto
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int DueMembers { get; set; }
        public int AdvanceMembers { get; set; }
        public decimal TotalDueAmount { get; set; }
        public decimal TotalAdvanceAmount { get; set; }
        public decimal TotalCollection { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class MonthlySummaryDto
    {
        public string MonthYear { get; set; }
        public int NewMembers { get; set; }
        public decimal TotalPayments { get; set; }
        public int TotalDueMembers { get; set; }
        public int TotalAdvanceMembers { get; set; }
    }
}
