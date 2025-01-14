using Financia.Domain.Repositories;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Services.LoggedUser;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase: IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _expenseWriteRepository;
        private readonly IExpensesReadOnlyRepository _expenseReadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggedUser _loggedUser;

        public DeleteExpenseUseCase(
            IExpensesWriteOnlyRepository expenseRepository,
            IExpensesReadOnlyRepository expenseReadRepository,
            ILoggedUser loggedUser,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _loggedUser = loggedUser;
            _expenseReadRepository = expenseReadRepository;
            _expenseWriteRepository = expenseRepository;
        }

        public async Task Execute(long id)
        {
            Domain.Entities.User loggedUser = await _loggedUser.Get();

            var expense = _expenseReadRepository.GetById(loggedUser, id);

            if (expense is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _expenseWriteRepository.Delete(id);

            await _unitOfWork.Commit();
        }
    }
}
