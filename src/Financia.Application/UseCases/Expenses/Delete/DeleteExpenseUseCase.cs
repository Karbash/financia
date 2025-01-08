using Financia.Domain.Repositories;
using Financia.Domain.Repositories.Expenses;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.Delete
{
    public class DeleteExpenseUseCase: IDeleteExpenseUseCase
    {
        private readonly IExpensesWriteOnlyRepository _expenseRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteExpenseUseCase(
            IExpensesWriteOnlyRepository expenseRepository,
            IUnitOfWork unitOfWork)
        {
            _expenseRepository = expenseRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Execute(long id)
        {
            var result = await _expenseRepository.Delete(id);
            if (result == false)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }

            await _unitOfWork.Commit();
        }
    }
}
