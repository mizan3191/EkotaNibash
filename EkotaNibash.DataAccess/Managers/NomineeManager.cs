namespace EkotaNibash.DataAccess
{
    public class NomineeManager : BaseDataManager, INominee
    {
        public NomineeManager(BoniyadiContext model) : base(model)
        {
        }

        public async Task<List<Nominee>> GetAllAsync(int memberId)
        {
            return await _dbContext.Nominees
                .Where(n => n.EkotaMemberId == memberId && !n.IsInactive)
                .Select(n => new
                {
                    Nominee = n,

                    LastDoc = _dbContext.MemberDocuments
                        .Where(d => d.DocumentTypeId == 8 && d.EkotaMemberId == memberId)
                        .OrderByDescending(d => d.Id)
                        .Select(d => new
                        {
                            d.File,
                            d.FileName
                        })
                        .FirstOrDefault()
                })
                .Select(x => new Nominee
                {
                    Id = x.Nominee.Id,
                    Name = x.Nominee.Name,
                    DateOfBirth = x.Nominee.DateOfBirth,
                    RelationshipWithMember = x.Nominee.RelationshipWithMember,
                    MobileNumber = x.Nominee.MobileNumber,
                    OptionalMobileNumber = x.Nominee.OptionalMobileNumber,
                    BloodGroup = x.Nominee.BloodGroup,
                    NIDNumber = x.Nominee.NIDNumber,
                    Address = x.Nominee.Address,
                    EkotaMemberId = x.Nominee.EkotaMemberId,
                    IsInactive = x.Nominee.IsInactive,

                    // ✅ ONE TIME document fetch
                    File = x.LastDoc != null ? x.LastDoc.File : null,
                    FileName = x.LastDoc != null ? x.LastDoc.FileName : null
                })
                .ToListAsync();
        }



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

            // If there's a file attached, create a MemberDocument
            if (nominee.File != null && nominee.File.Length > 0)
            {
                CreateNomineeDocument(nominee);
            }
        }


        private void CreateNomineeDocument(Nominee nominee)
        {
            var memberDocument = new MemberDocument
            {
                EkotaMemberId = nominee.EkotaMemberId,
                NomineeId = nominee.Id,
                DocumentTypeId = 8, // Assuming 6 is "Photo" from your DocumentTypes
                DocumentName = "Profile Picture",
                Description = $"Profile Picture of {nominee.Name} on {DateTime.Now:dd-MMM-yyyy}",
                FileName = nominee.FileName,
                FileType = Path.GetExtension(nominee.FileName),
                File = nominee.File,
                DocumentDate = DateTime.Now,
            };

            _dbContext.MemberDocuments.Add(memberDocument);
            _dbContext.SaveChanges();
        }

        public bool DeleteNominee(int id)
        {
            ExecuteSqlInterpolated($"UPDATE Nominees SET IsInactive = 1 WHERE Id = {id}");
            return true;
        }
    }
}
