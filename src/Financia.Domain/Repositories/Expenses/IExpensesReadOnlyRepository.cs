using Financia.Domain.Entities;

namespace Financia.Domain.Repositories.Expenses
{
    public interface IExpensesReadOnlyRepository
    {
        public Task<List<Expense>> GetAll(int page, int pageSize, Entities.User user);
        public Task<Expense?> GetById(Entities.User user,long id);
        public Task<List<Expense>> FilterByMonth(Entities.User user, DateOnly date);
    }
}
