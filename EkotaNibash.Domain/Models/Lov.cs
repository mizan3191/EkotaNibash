namespace EkotaNibash.Domain
{
    public class Lov
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Lovd : Lov
    {
        public string Desc { get; set; }
    }

    public class UniqueMemberDTO : Lov
    {
        public string PhoneNumber { get; set; }
        public string MembershipNo { get; set; }
    }
}
