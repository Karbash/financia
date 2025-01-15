﻿namespace Financia.Domain.Repositories.User
{
    public interface IUserUpdateOnlyRepository
    {
        Task<Entities.User?> GetById(long id);
        void Update(Entities.User user);
    }
}
