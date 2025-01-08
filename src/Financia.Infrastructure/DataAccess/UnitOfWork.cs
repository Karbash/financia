using Financia.Domain.Repositories;

namespace Financia.Infrastructure.DataAccess
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly FinanciaDBContext _dbContext;

        public UnitOfWork(FinanciaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Commit()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
