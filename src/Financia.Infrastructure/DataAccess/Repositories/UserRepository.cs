using Financia.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess.Repositories
{
    internal class UserRepository : IUserReadOnlyRepository
    {
        private readonly FinanciaDBContext _dbContext;

        public UserRepository(FinanciaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }
    }
}
