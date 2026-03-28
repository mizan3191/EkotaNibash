namespace EkotaNibash.DataAccess
{
    public interface IProject
    {
        #region Project
        int CreateProject(Project project);
        bool UpdateProject(Project project);
        bool DeleteProject(int id);
        Project GetProject(int id);
        Task<IList<Project>> GetAllProjects();
        string GetProjectName(int id);
        #endregion Project

        #region ProjectExpenseDetails
        int ProjectExpenseDetailsCreate(ProjectExpenseDetails entity);
        bool ProjectExpenseDetailsUpdate(ProjectExpenseDetails entity);
        bool ProjectExpenseDetailsDelete(int id);
        ProjectExpenseDetails GetProjectExpenseDetails(int id);
        Task<IList<ProjectExpenseDetails>> GetAllProjectExpenseDetails(int projectId, DateTime? startDate, DateTime? endDate);
        #endregion ProjectExpenseDetails

        #region Project Member
        int ProjectMemberCreate(ProjectMember entity);
        Task AddProjectMembers(int projectId, List<int> memberIds);
        bool ProjectMemberDelete(int id);
        ProjectMember GetProjectMember(int id);
        Task<IList<ProjectMember>> GetProjectMembers(int projectId);
        Task<IList<UniqueMemberDTO>> GetUniqueProjectMembers(int projectId);

        Task<IList<Lov>> GetAllProjectMembers(int projectId);
        #endregion Project Member 

        #region
        int ProjectPaymentCreate(ProjectPayment entity);
        bool ProjectPaymentUpdate(ProjectPayment entity);
        bool ProjectPaymentDelete(int id);
        ProjectPayment GetProjectPayment(int id);
        Task<IList<ProjectPayment>> GetAllProjectPayments(int projectId, DateTime? startDate, DateTime? endDate);
        #endregion 


        IQueryable<ProjectDocumentListDTO> GetAllProjectDocuments(int projectId);

        int ProjectDocumentCreate(ProjectDocument project);
        ProjectDocument GetDocument(int id);

        ProjectDocument DownloadDocument(int id);

        bool DeleteDocument(int id);
    }
}
