namespace EkotaNibash.DataAccess
{
    public interface IDashboard
    {
        Task<DashboardSummary> GetDashboardSummaryAsync();
        Task<List<MonthlySummary>> GetMonthlySummaryAsync(int months = 12);
        Task<List<MemberDueDto>> GetMembersWithDueAsync();
        Task<List<MemberAdvanceDto>> GetMembersWithAdvanceAsync();
    }
}
