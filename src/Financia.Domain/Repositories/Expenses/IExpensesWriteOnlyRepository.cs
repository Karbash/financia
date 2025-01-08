using Financia.Domain.Entities;

namespace Financia.Domain.Repositories.Expenses
{
    public interface IExpensesWriteOnlyRepository
    {
        public Task Add(Expense expense);
        /// <summary>
        /// This function return TRUE if the deletion was successfull otherwise returns FALSE.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Boolean</returns>
        public Task<bool> Delete(long id);

    }
}
