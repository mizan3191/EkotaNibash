namespace EkotaNibash.Domain
{
    public class CompanyInfo
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }
        public string WebSiteLink { get; set; }
        public string Email { get; set; }
        public string HeadOfficeAddress { get; set; }
        public string SpecialNotice { get; set; }
        public string FacebookPage { get; set; }
        public string Logo { get; set; }
        public byte[]? Image { get; set; }
        public string CompanyMessage { get; set; }
    }
}