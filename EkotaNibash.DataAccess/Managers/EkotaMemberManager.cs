namespace EkotaNibash.DataAccess
{
    public class EkotaMemberManager : BaseDataManager, IEkotaMember
    {
        public EkotaMemberManager(BoniyadiContext model) : base(model)
        {
        }

        public async Task<List<EkotaMember>> GetAllAsync()
        {
            return await _dbContext.EkotaMembers
                .Select(m => new
                {
                    Member = m,

                    LastDoc = _dbContext.MemberDocuments
                        .Where(d =>
                            d.DocumentTypeId == 8 &&
                            d.EkotaMemberId == m.Id
                        )
                        .OrderByDescending(d => d.Id) // or CreatedDate
                        .Select(d => new
                        {
                            d.File,
                            d.FileName
                        })
                        .FirstOrDefault()
                })
                .Select(x => new EkotaMember
                {
                    Id = x.Member.Id,
                    MembershipNo = x.Member.MembershipNo,
                    ApplicationDate = x.Member.ApplicationDate,
                    Name = x.Member.Name,
                    DateOfBirth = x.Member.DateOfBirth,
                    NIDNumber = x.Member.NIDNumber,
                    FatherName = x.Member.FatherName,
                    MotherName = x.Member.MotherName,
                    Occupation = x.Member.Occupation,
                    MobileNumber = x.Member.MobileNumber,
                    OptionalMobileNumber = x.Member.OptionalMobileNumber,
                    Email = x.Member.Email,
                    BloodGroup = x.Member.BloodGroup,
                    PresentAddress = x.Member.PresentAddress,
                    PermanentAddress = x.Member.PermanentAddress,
                    IsDeclarationAccepted = x.Member.IsDeclarationAccepted,
                    DeclarationDate = x.Member.DeclarationDate,
                    IsInactive = x.Member.IsInactive,

                    // ✅ ONE TIME document fetch
                    File = x.LastDoc != null ? x.LastDoc.File : null,
                    FileName = x.LastDoc != null ? x.LastDoc.FileName : null
                })
                .ToListAsync();
        }


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

            // If there's a file attached, create a MemberDocument
            if (member.File != null && member.File.Length > 0)
            {
                CreateEkotaMemberDocument(member);
            }
        }

        public bool DeleteMember(int memberId)
        {
            ExecuteSqlInterpolated($"UPDATE EkotaMembers SET IsInactive = 1 WHERE Id = {memberId}");
            return true;
        }

        public bool UndoMember(int memberId)
        {
            ExecuteSqlInterpolated($"UPDATE EkotaMembers SET IsInactive = 0 WHERE Id = {memberId}");
            return true;
        }


        private void CreateEkotaMemberDocument(EkotaMember member)
        {
            var memberDocument = new MemberDocument
            {
                EkotaMemberId = member.Id,
                DocumentTypeId = 8, // Assuming 6 is "Photo" from your DocumentTypes
                DocumentName = "Profile Picture",
                Description = $"Profile Picture of {member.Name} on {DateTime.Now:dd-MMM-yyyy}",
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
