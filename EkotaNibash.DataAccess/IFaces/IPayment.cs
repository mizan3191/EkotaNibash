namespace EkotaNibash.DataAccess
{
    public interface IPayment
    {
        Task<List<MembershipPayment>> GetAllAsync(int memberId);
        Task<MembershipPayment> GetByIdAsync(int id);
        Task AddAsync(MembershipPayment payment);
        Task UpdateAsync(MembershipPayment payment);
    }
}
