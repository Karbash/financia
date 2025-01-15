using Financia.Domain.Repositories;
using Financia.Domain.Repositories.User;
using Financia.Domain.Services.LoggedUser;

namespace Financia.Application.UseCases.Users.Delete
{
    public class DeleteUserAccountUseCase: IDeleteUserAccountUseCase
    {
        private readonly IUserWriteOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserAccountUseCase(IUnitOfWork unitOfWork, IUserWriteOnlyRepository repository, ILoggedUser loggedUser)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _loggedUser = loggedUser;
        }

        public async Task Execute()
        {
            var user = await _loggedUser.Get();
            await _repository.Delete(user);
            await _unitOfWork.Commit();
        }
}
