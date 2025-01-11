using Financia.Domain.Entities;
using Financia.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess.Repositories
{
    internal class UserRepository : IUserReadOnlyRepository,
                                    IUserWriteOnlyRepository
    {
        private readonly FinanciaDBContext _dbContext;

        public UserRepository(FinanciaDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(User user)
        {
            await _dbContext.Users.AddAsync(user);
        }

        public async Task<bool> ExistActiveUserWithEmail(string email)
        {
            return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
        }

        public async Task<User?> GetUserByEmail(string email)
        {
           return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
        }
    }
}
