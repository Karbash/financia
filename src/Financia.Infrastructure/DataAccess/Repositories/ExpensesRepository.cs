﻿
using Financia.Domain.Entities;
using Financia.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

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

        public async Task Delete(long id)
        {
            var result = await _dbContext.Expenses.FindAsync(id);
            _dbContext.Expenses.Remove(result!);
 
        }

        public async Task<List<Expense>> GetAll(int page, int pageSize, User user)
        {
            return await _dbContext.Expenses
                        .AsNoTracking()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .Where(expense => expense.UserId == user.Id)
                        .ToListAsync();
        }

        async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user, long id)
        {
            return await GetFullExpense()
                .AsNoTracking()
                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
        }

        async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user , long id)
        {
            return await GetFullExpense()
                .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
        }

        public void Update(Expense expense)
        {
            _dbContext.Expenses.Update(expense);
        }

        public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
        {
            var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
            int daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
            var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);

            return await _dbContext
               .Expenses
               .AsNoTracking()
               .Where(expense => expense.Date >= startDate && expense.Date <= endDate && expense.UserId == user.Id)
               .OrderBy(expense => expense.Date)
               .ThenBy(expense => expense.Title)
               .ToListAsync(); 
        }

        private IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
        {
            return _dbContext.Expenses
                .Include(expense => expense.Tags);
        }
    }
}
