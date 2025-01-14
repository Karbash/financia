using AutoMapper;
using Financia.Communication.Responses;
using Financia.Domain.Entities;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Services.LoggedUser;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.ById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;

        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository,ILoggedUser loggedUser, IMapper mapper)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
            _loggedUser = loggedUser;
        }

        public async Task<ResponseExpenseJson> Execute(long id)
        {
            var loggedUser = await _loggedUser.Get();
            Expense? result = await _expensesRepository.GetById(loggedUser, id);

            if (result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }
            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
