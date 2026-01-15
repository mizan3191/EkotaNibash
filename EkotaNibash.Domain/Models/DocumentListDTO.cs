namespace EkotaNibash.Domain
{
    public class DocumentListDTO
    {
        public int Id { get; set; }
        public int EkotaMemberId { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public int? DocumentTypeId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string Comments { get; set; }
    }

    public class CommonDocumentDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }

        public byte[] File { get; set; }
    }
}
