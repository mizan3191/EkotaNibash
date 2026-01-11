namespace EkotaNibash.DataAccess
{
    public interface IEkotaMember
    {
        Task<List<EkotaMember>> GetAllAsync();
        Task<EkotaMember> GetByIdAsync(int id);
        Task AddAsync(EkotaMember member);
        Task UpdateAsync(EkotaMember member);
        bool DeleteMember(int memberId);
    }
}
