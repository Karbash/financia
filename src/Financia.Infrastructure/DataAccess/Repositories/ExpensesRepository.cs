
using Financia.Domain.Entities;
using Financia.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess.Repositories
{
    internal class ExpensesRepository : IExpensesReadOnlyRepository, 
                                        IExpensesWriteOnlyRepository,
                                        IExpensesUpdateOnlyRepository
    {
        private readonly FinanciaDBContext _dbContext;

        public ExpensesRepository(FinanciaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Expense expense)
        {
            await _dbContext.Expenses.AddAsync(expense);
        }

        public async Task<bool> Delete(long id)
        {
            var result = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
            if (result is null)
            {
                return false;
            }
            _dbContext.Expenses.Remove(result);
            return true;
        }

        public async Task<List<Expense>> GetAll(int page, int pageSize)
        {
            return await _dbContext.Expenses
                        .AsNoTracking()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
        }

        async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
        {
            return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
        }

        async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
        {
            return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
        }

        public void Update(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
        }

        public async Task<List<Expense>> FilterByMonth(DateOnly date)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
            int daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
            var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

            return await _dbContext
               .Expenses
               .AsNoTracking()
               .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
               .OrderBy(expense => expense.Date)
               .ThenBy(expense => expense.Title)
               .ToListAsync(); 
        }
    }
}
