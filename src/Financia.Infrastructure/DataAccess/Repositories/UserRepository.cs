using Financia.Domain.Entities;
using Financia.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace Financia.Infrastructure.DataAccess.Repositories
{
    internal class UserRepository : IUserReadOnlyRepository,
                                    IUserWriteOnlyRepository,
                                    IUserUpdateOnlyRepository
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

        public async Task<User?> GetById(long id)
        {
            return await _dbContext.Users.FirstAsync(user => user.Id == id);
        }

        public void Update(User user)
        {
            _dbContext.Users.Update(user);
        }

        public async Task Delete(User user)
        {
            var userToRemove = await _dbContext.Users.FindAsync(user.Id);
            _dbContext.Users.Remove(userToRemove!);
        }
    }
}
