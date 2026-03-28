namespace EkotaNibash.DataAccess
{
    public class ProjectManager : BaseDataManager, IProject
    {
        public ProjectManager(BoniyadiContext model) : base(model)
        {
        }

        #region Project
        public int CreateProject(Project project)
        {
            AddUpdateEntity(project);
            return project.Id;
        }

        public bool DeleteProject(int id)
        {
            return RemoveEntity<Project>(id);
        }

        public async Task<IList<Project>> GetAllProjects()
        {
            return await _dbContext.Projects.ToListAsync();
        }

        public Project GetProject(int id)
        {
            return FindEntity<Project>(id);
        }

        public bool UpdateProject(Project project)
        {
            return AddUpdateEntity(project);
        }

        #endregion Project

        #region Project Expense Details
        public int ProjectExpenseDetailsCreate(ProjectExpenseDetails entity)
        {
            AddUpdateEntity(entity);
            return entity.Id;
        }

        public bool ProjectExpenseDetailsDelete(int id)
        {
            return RemoveEntity<ProjectExpenseDetails>(id);
        }

        public ProjectExpenseDetails GetProjectExpenseDetails(int id)
        {
            return FindEntity<ProjectExpenseDetails>(id);
        }

        public async Task<IList<ProjectExpenseDetails>> GetAllProjectExpenseDetails(int projectId, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.ProjectExpenseDetails
                .Where(x => x.ProjectId == projectId)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.Date <= endDate.Value);

            return await query.ToListAsync();
        }

        public bool ProjectExpenseDetailsUpdate(ProjectExpenseDetails entity)
        {
            return AddUpdateEntity(entity);
        }
        #endregion Project Expense Details

        #region Project Member
        public int ProjectMemberCreate(ProjectMember entity)
        {
            AddUpdateEntity(entity);
            return entity.Id;
        }

        public bool ProjectMemberDelete(int id)
        {
            return RemoveEntity<ProjectMember>(id);
        }

        public ProjectMember GetProjectMember(int id)
        {
            return FindEntity<ProjectMember>(id);
        }

        public string GetProjectName(int id)
        {
            return _dbContext.Projects.FirstOrDefault(x => x.Id == id).Name;
        }

        public async Task<IList<ProjectMember>> GetProjectMembers(int projectId)
        {
            return await _dbContext.ProjectMembers
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.EkotaMember)
                .Include(x => x.Project)
                .ToListAsync();
        }

        #endregion Project Member

        #region Project Payment
        public int ProjectPaymentCreate(ProjectPayment entity)
        {
            AddUpdateEntity(entity);
            return entity.Id;
        }

        public bool ProjectPaymentDelete(int id)
        {
            return RemoveEntity<ProjectPayment>(id);
        }

        public ProjectPayment GetProjectPayment(int id)
        {
            return FindEntity<ProjectPayment>(id);
        }

        public async Task<IList<ProjectPayment>> GetAllProjectPayments(int projectId, DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.ProjectPayments
                .Where(x => x.ProjectId == projectId)
                .Include(x => x.EkotaMember)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(x => x.Date >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(x => x.Date <= endDate.Value);

            return await query.ToListAsync();
        }

        public bool ProjectPaymentUpdate(ProjectPayment entity)
        {
            return AddUpdateEntity(entity);
        }
        #endregion Project Payment
    }
}