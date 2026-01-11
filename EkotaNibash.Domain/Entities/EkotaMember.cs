

namespace EkotaNibash.Domain
{
    public class EkotaMember
    {
        public EkotaMember()
        {
            MembershipPayments = new HashSet<MembershipPayment>();
        }
        public int Id { get; set; }

        // Membership Info
        public string MembershipNo { get; set; }
        public DateTime ApplicationDate { get; set; }

        // Member Personal Info

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "NID Number")]
        [Required(ErrorMessage = "NID Number is required")]
        [StringLength(100)]
        public string NIDNumber { get; set; }

        [Display(Name = "Father's Name")]
        [Required(ErrorMessage = "Father's Name is required")]
        [StringLength(100)]
        public string FatherName { get; set; }

        [Display(Name = "Mother's Name")]
        [Required(ErrorMessage = "Mother's Name is required")]
        [StringLength(100)]
        public string MotherName { get; set; }

        public string Occupation { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        public string OptionalMobileNumber { get; set; }
        public string Email { get; set; }

        public string BloodGroup { get; set; }

        // Address
        [Display(Name = "Present Address")]
        [StringLength(1000)]
        public string PresentAddress { get; set; }

        [Display(Name = "Present Address")]
        [StringLength(1000)]
        public string PermanentAddress { get; set; }

        // Photo
        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        public byte[] File { get; set; }

        // Add this property
        [NotMapped]
        public string PhotoBase64
        {
            get
            {
                if (File != null && File.Length > 0)
                {
                    var base64 = Convert.ToBase64String(File);
                    var mimeType = GetMimeType(FileName);
                    return $"data:{mimeType};base64,{base64}";
                }
                return null;
            }
        }

        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "image/pdf",
                ".docx" => "image/docx",
                ".doc" => "image/doc",
                ".xlsx" => "image/xlsx",
                ".txt" => "image/txt",
                ".pptx" => "image/pptx",
                ".ppt" => "image/ppt",
                ".rtf" => "image/rtf",
                _ => "application/octet-stream"
            };
        }

        // Nominee
        public virtual ICollection<MembershipPayment> MembershipPayments { get; set; }

        // Declaration
        public bool IsDeclarationAccepted { get; set; }
        public DateTime? DeclarationDate { get; set; }

        public bool IsInactive { get; set; }
    }

    public class Nominee
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name is required")]
        [StringLength(200)]
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string RelationshipWithMember { get; set; }

        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is required")]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        public string OptionalMobileNumber { get; set; }
        public string BloodGroup { get; set; }

        [Display(Name = "NID Number")]
        [Required(ErrorMessage = "NID Number is required")]
        [StringLength(50)]
        public string NIDNumber { get; set; }
        public string Address { get; set; }
        public int EkotaMemberId { get; set; }
        public EkotaMember EkotaMember { get; set; }

        // Photo
        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        public byte[] File { get; set; }

        // Add this property
        [NotMapped]
        public string PhotoBase64
        {
            get
            {
                if (File != null && File.Length > 0)
                {
                    var base64 = Convert.ToBase64String(File);
                    var mimeType = GetMimeType(FileName);
                    return $"data:{mimeType};base64,{base64}";
                }
                return null;
            }
        }

        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "image/pdf",
                ".docx" => "image/docx",
                ".doc" => "image/doc",
                ".xlsx" => "image/xlsx",
                ".txt" => "image/txt",
                ".pptx" => "image/pptx",
                ".ppt" => "image/ppt",
                ".rtf" => "image/rtf",
                _ => "application/octet-stream"
            };
        }


        public string NomineePhotoPath { get; set; }
        public bool IsInactive { get; set; }
    }

    public class MembershipPayment
    {
        public int Id { get; set; }

        public int EkotaMemberId { get; set; }
        public EkotaMember EkotaMember { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime PaymentDate { get; set; }

        public string Notes { get; set; }

        [NotMapped]
        public string FileName { get; set; }

        [NotMapped]
        public byte[] File { get; set; }

        // Add this property
        [NotMapped]
        public string PhotoBase64
        {
            get
            {
                if (File != null && File.Length > 0)
                {
                    var base64 = Convert.ToBase64String(File);
                    var mimeType = GetMimeType(FileName);
                    return $"data:{mimeType};base64,{base64}";
                }
                return null;
            }
        }

        private string GetMimeType(string fileName)
        {
            var extension = Path.GetExtension(fileName)?.ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".pdf" => "image/pdf",
                ".docx" => "image/docx",
                ".doc" => "image/doc",
                ".xlsx" => "image/xlsx",
                ".txt" => "image/txt",
                ".pptx" => "image/pptx",
                ".ppt" => "image/ppt",
                ".rtf" => "image/rtf",
                _ => "application/octet-stream"
            };
        }

    }

    public enum PaymentType
    {
        [Display(Name = "Monthly"), Description("Monthly")]
        RegularMonthlyPayment = 1,

        [Display(Name = "AdmissionFee"), Description("AdmissionFee")]
        AdmissionFee = 2,

        [Display(Name = "Penalty"), Description("Penalty")]
        Penalty = 3,

        [Display(Name = "Donation"), Description("Donation")]
        Donation = 4,

        [Display(Name = "Installment"), Description("Installment")]
        Installment = 5,

        [Display(Name = "Other"), Description("Other")]
        Other = 6,
    }
}
