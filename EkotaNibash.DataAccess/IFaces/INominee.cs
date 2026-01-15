namespace EkotaNibash.DataAccess
{
    public interface INominee
    {
        Task<List<Nominee>> GetAllAsync(int memberId);
        Task<Nominee> GetByIdAsync(int id);
        Task AddAsync(Nominee nominee);
        Task UpdateAsync(Nominee nominee);
        bool DeleteNominee(int id);
    }
}
