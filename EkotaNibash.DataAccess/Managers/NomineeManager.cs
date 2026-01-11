namespace EkotaNibash.DataAccess
{
    public class NomineeManager : BaseDataManager, INominee
    {
        public NomineeManager(BoniyadiContext model) : base(model)
        {
        }

        public Task<List<Nominee>> GetAllAsync(int memberId) =>
            _dbContext.Nominees
            .Where(x => x.EkotaMemberId == memberId)
            .ToListAsync();

        public Task<Nominee> GetByIdAsync(int id) =>
            _dbContext.Nominees
            .FindAsync(id)
            .AsTask();


        public async Task AddAsync(Nominee nominee)
        {
            _dbContext.Nominees.Add(nominee);
            await _dbContext.SaveChangesAsync();

            // If there's a file attached, create a MemberDocument
            if (nominee.File != null && nominee.File.Length > 0)
            {
                CreateNomineeDocument(nominee);
            }
        }


        public async Task UpdateAsync(Nominee nominee)
        {
            _dbContext.Nominees.Update(nominee);
            await _dbContext.SaveChangesAsync();
        }


        private void CreateNomineeDocument(Nominee nominee)
        {
            var memberDocument = new MemberDocument
            {
                EkotaMemberId = nominee.EkotaMemberId,
                NomineeId = nominee.Id,
                DocumentTypeId = 2, // Assuming 6 is "Photo" from your DocumentTypes
                DocumentName = $"Nominee Information - {DateTime.Now:dd-MMM-yyyy}",
                Description = $"Document of ৳{nominee.Name} on {DateTime.Now:dd-MMM-yyyy}",
                FileName = nominee.FileName,
                FileType = Path.GetExtension(nominee.FileName),
                File = nominee.File,
                DocumentDate = DateTime.Now,
            };

            _dbContext.MemberDocuments.Add(memberDocument);
            _dbContext.SaveChanges();
        }

    }
}
