using System.ComponentModel.DataAnnotations;

namespace EkotaNibash.Domain
{
    public class MemberDocument
    {
        public int Id { get; set; }

        public int EkotaMemberId { get; set; }
        public EkotaMember EkotaMember { get; set; }

        public int? NomineeId { get; set; }
        public Nominee Nominee { get; set; }

        public int? MembershipPaymentId { get; set; }
        public MembershipPayment MembershipPayment { get; set; }

        public int DocumentTypeId { get; set; }
        public DocumentType DocumentType { get; set; }

        [Display(Name = "Document Name")]
        [Required(ErrorMessage = "Document Name is required")]
        [StringLength(100, ErrorMessage = "Document Name exceeded the max {1} characters")]
        public string DocumentName { get; set; }

        [Display(Name = "Description")]
        [StringLength(4000, ErrorMessage = "Description exceeded the max {1} characters")]
        public string Description { get; set; }

        public string FileType { get; set; }

        public string FileName { get; set; }

        public byte[] File { get; set; }

        [Display(Name = "Document Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Document Date is not a valid date")]
        public DateTime? DocumentDate { get; set; }


        private int SliceSize = 7340032; // 7MB -- 10485760; //10 MB -- 3145728; // 3MB -- 5242880; // 5MB -- 2097152; // 2MB
        private byte[] fileBytes;
        public bool IsFileValid(string filename, byte[] data, string fileType)
        {
            string fileName = filename.ToLower();

            if (fileName.LastIndexOf(".jpeg") <= 0 &&
                fileName.LastIndexOf(".jpg") <= 0 &&
                fileName.LastIndexOf(".png") <= 0 &&
                fileName.LastIndexOf(".bmp") <= 0 &&
                fileName.LastIndexOf(".pdf") <= 0 &&
                fileName.LastIndexOf(".docx") <= 0 &&
                fileName.LastIndexOf(".doc") <= 0 &&
                fileName.LastIndexOf(".xlsx") <= 0 &&
                fileName.LastIndexOf(".txt") <= 0 &&
                fileName.LastIndexOf(".pptx") <= 0 &&
                fileName.LastIndexOf(".ppt") <= 0 &&
                fileName.LastIndexOf(".rtf") <= 0)
            {
                return false;
            }

            File = data;
            FileName = filename;
            FileType = fileType;

            return true;
        }
        public bool HasSlices
        {
            get
            {
                return

                    File == null || File.Length > SliceSize;
            }
        }

        public void PrepareFirstSlice()
        {
            fileBytes = File;
            File = fileBytes.Take(SliceSize).ToArray();
        }

        public List<S> GetDocumentSlices<S>() where S : MemberDocumentSliceBase, new()
        {

            List<S> slices = new();

            int totalLength = fileBytes.Length;
            int remainingLength = totalLength;
            int slicesRead = 0;
            int order = 0;

            if (totalLength > SliceSize)
            {
                slicesRead = slicesRead + SliceSize;

                while (totalLength >= slicesRead)
                {
                    order = order + 1;

                    byte[] ExtraSlice = fileBytes.Skip(slicesRead).Take(SliceSize).ToArray();
                    slices.Add(new S() { DocSlices = ExtraSlice, Order = order });

                    slicesRead = slicesRead + SliceSize;
                }
            }

            return slices;
        }
    }

    public abstract class MemberDocumentSliceBase
    {
        public byte[]? DocSlices { get; set; }

        public int? Order { get; set; }
    }

    public class MemberDocumentSlices : MemberDocumentSliceBase
    {
        public int Id { get; set; }
        public int MemberDocumentId { get; set; }
        public virtual MemberDocument MemberDocument { get; set; }
    }

}
