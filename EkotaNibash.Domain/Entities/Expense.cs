using System.ComponentModel.DataAnnotations;

namespace EkotaNibash.Domain
{
    public class Expense
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;
        public string DateFormatted => ExpenseDate.ToString("dd-MMM-yyyy (ddd)");

        [Required]
        public int ExpenseTypeId { get; set; }
        public ExpenseType ExpenseType { get; set; } // e.g., Electricity Bill, Salary

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public double Amount { get; set; }

        [StringLength(100)]
        public string ReferenceNumber { get; set; } // Bill or invoice number

        public bool IsDeleted { get; set; } = false;
    }
}