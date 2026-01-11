namespace EkotaNibash.DataAccess
{
    public class LookupManager : BaseDataManager, ILookup
    {
        public LookupManager(BoniyadiContext model) : base(model)
        {
        }

        public async Task<IList<Lov>> GetAllExpenseTypeList()
        {
            return await _dbContext.ExpenseType
                .Select(x => new Lov
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public async Task<IList<Lov>> GetAllDocumentTypeList()
        {
            return await _dbContext.DocumentType
                .Select(x => new Lov
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync();
        }

        public (string CompanyName, string Address) GetCompanyInfo()
        {
            var company = _dbContext.CompanyInfos.FirstOrDefault();

            if (company == null)
                return (string.Empty, string.Empty);

            return (company.Name, company.HeadOfficeAddress);
        }
    }
}
