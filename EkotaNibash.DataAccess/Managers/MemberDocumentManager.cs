namespace EkotaNibash.DataAccess
{
    public class MemberDocumentManager : BaseDataManager, IMemberDocument
    {
        public MemberDocumentManager(BoniyadiContext context) : base(context) { }

        public MemberDocument DownloadDocument(int documentId)
        {
            try
            {
                var doc = FindEntity<MemberDocument>(documentId);
                byte[] fileArray = doc.File;

                var slices = _dbContext.MemberDocumentSlices.FromSqlInterpolated($"SELECT * FROM MemberDocumentSlices WHERE MemberDocumentId={doc.Id} ORDER BY [Order]").AsNoTracking().ToList();

                foreach (var slice in slices)
                {
                    fileArray = fileArray.Concat(slice.DocSlices).ToArray();
                }

                doc.File = fileArray;
                return doc;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }

        }

        public bool DeleteDocument(int id)
        {
            ExecuteSqlInterpolated($"DELETE FROM MemberDocumentSlices WHERE MemberDocumentId = {id}");
            ExecuteSqlInterpolated($"DELETE FROM MemberDocuments WHERE Id = {id}");
            return true;
        }

        public MemberDocument GetDocument(int id)
        {
            return FindEntity<MemberDocument>(id);
        }

        public IList<DocumentListDTO> GetAllMemberDocuments(int memberId)
        {
            try
            {

                var qry = from docs in _dbContext.MemberDocuments
                          .Include(x => x.DocumentType)
                       .Where(x => x.EkotaMemberId == memberId)
                          select new DocumentListDTO
                          {
                              Id = docs.Id,
                              EkotaMemberId = docs.EkotaMemberId,
                              DocumentType = docs.DocumentType.Description,
                              DocumentTypeId = docs.DocumentTypeId,
                              DocumentName = docs.DocumentName,
                              DocumentDate = docs.DocumentDate.Value,
                              Comments = docs.Description,

                          };

                return qry.OrderByDescending(x => x.Id)
                    .AsNoTracking()
                    .ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString()); throw;
            }
        }

        public int CreateDocument(MemberDocument documentData)
        {
            try
            {
                _dbContext.Database.BeginTransaction();

                if (documentData.HasSlices != true)
                {
                    AddUpdateEntity<MemberDocument>(documentData);
                }
                else
                {
                    documentData.PrepareFirstSlice();
                    AddUpdateEntity<MemberDocument>(documentData);

                    List<MemberDocumentSlices> sliceList = documentData.GetDocumentSlices<MemberDocumentSlices>();
                    foreach (MemberDocumentSlices slice in sliceList)
                    {
                        slice.MemberDocumentId = documentData.Id;
                        AddUpdateEntity<MemberDocumentSlices>(slice);
                    }
                }

                _dbContext.Database.CommitTransaction();
                return documentData.Id;
            }
            catch (InvalidOperationException)
            {
                _dbContext.Database.RollbackTransaction();
                return 0;
            }
            catch (DbUpdateException)
            {
                _dbContext.Database.RollbackTransaction();
                return 0;
            }
            catch (Exception ex)
            {
                _dbContext.Database.RollbackTransaction();
                throw;

            }
        }
    }
}