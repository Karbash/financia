using Financia.Domain.Entities;

namespace Financia.Domain.Repositories.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        public Task Add(Expense expense);
        public Task Delete(long id);

    }
}
