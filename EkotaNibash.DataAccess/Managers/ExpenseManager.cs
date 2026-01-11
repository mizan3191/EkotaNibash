namespace EkotaNibash.DataAccess
{
    public class ExpenseManager : BaseDataManager, IExpense
    {
        public ExpenseManager(BoniyadiContext model) : base(model)
        {
        }

        public int CreateExpense(Expense expense)
        {
            AddUpdateEntity(expense);
            return expense.Id;
        }

        public bool DeleteExpense(int id)
        {
            return RemoveEntity<Expense>(id);
        }

        public async Task<IList<Expense>> GetAllExpenses(DateTime? startDate, DateTime? endDate)
        {
            var query = _dbContext.Expenses
                .Include(x => x.ExpenseType)
                .AsQueryable();

            if (startDate.HasValue)
            {
                query = query.Where(x => x.ExpenseDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.ExpenseDate <= endDate.Value);
            }

            return await query.ToListAsync();
        }

        public Expense GetExpense(int id)
        {
            return FindEntity<Expense>(id);
        }

        public bool UpdateExpense(Expense expense)
        {
            return AddUpdateEntity(expense);
        }
    }
}