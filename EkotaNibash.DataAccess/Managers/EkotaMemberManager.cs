namespace EkotaNibash.DataAccess
{
    public class EkotaMemberManager : BaseDataManager, IEkotaMember
    {
        public EkotaMemberManager(BoniyadiContext model) : base(model)
        {
        }

        public Task<List<EkotaMember>> GetAllAsync() => _dbContext.EkotaMembers
            .Where(m => !m.IsInactive)
            .ToListAsync();

        public Task<EkotaMember> GetByIdAsync(int id) => _dbContext.EkotaMembers.FindAsync(id).AsTask();


        public async Task AddAsync(EkotaMember member)
        {
            _dbContext.EkotaMembers.Add(member);
            await _dbContext.SaveChangesAsync();

            // If there's a file attached, create a MemberDocument
            if (member.File != null && member.File.Length > 0)
            {
                CreateEkotaMemberDocument(member);
            }
        }


        public async Task UpdateAsync(EkotaMember member)
        {
            _dbContext.EkotaMembers.Update(member);
            await _dbContext.SaveChangesAsync();
        }

        public bool DeleteMember(int memberId)
        {
            ExecuteSqlInterpolated($"UPDATE EkotaMembers SET IsInactive = 1 WHERE Id = {memberId}");
            return true;
        }


        private void CreateEkotaMemberDocument(EkotaMember member)
        {
            var memberDocument = new MemberDocument
            {
                EkotaMemberId = member.Id,
                DocumentTypeId = 2, // Assuming 6 is "Photo" from your DocumentTypes
                DocumentName = $"Nominee Information - {DateTime.Now:dd-MMM-yyyy}",
                Description = $"Document of ৳{member.Name} on {DateTime.Now:dd-MMM-yyyy}",
                FileName = member.FileName,
                FileType = Path.GetExtension(member.FileName),
                File = member.File,
                DocumentDate = DateTime.Now,
            };

            _dbContext.MemberDocuments.Add(memberDocument);
            _dbContext.SaveChanges();
        }

    }
}
