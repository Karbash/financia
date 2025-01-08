using Financia.Domain.Entities;

namespace Financia.Domain.Repositories.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        public Task<List<Expense>> GetAll(int page, int pageSize);
        public Task<Expense?> GetById(long id);
        public Task<List<Expense>> FilterByMonth(DateOnly date);
    }
}
