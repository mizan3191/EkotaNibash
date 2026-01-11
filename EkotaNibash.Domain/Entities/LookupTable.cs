namespace EkotaNibash.Domain
{
    public abstract class BaseLookup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class ExpenseType : BaseLookup { }
    public class DocumentType : BaseLookup { }
}
