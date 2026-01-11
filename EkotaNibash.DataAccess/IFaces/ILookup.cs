namespace EkotaNibash.DataAccess
{
    public interface ILookup
    {
        (string CompanyName,string Address ) GetCompanyInfo();
        Task<IList<Lov>> GetAllExpenseTypeList();
        Task<IList<Lov>> GetAllDocumentTypeList();
    }
}