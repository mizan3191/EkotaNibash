namespace EkotaNibash.DataAccess
{
    public interface IMemberDocument
    {
        MemberDocument DownloadDocument(int documentId);
        bool DeleteDocument(int documentId);
        MemberDocument GetDocument(int documentId);
        IList<DocumentListDTO> GetAllMemberDocuments(int clientId);
        int CreateDocument(MemberDocument report_DocumentData);
    }
}
