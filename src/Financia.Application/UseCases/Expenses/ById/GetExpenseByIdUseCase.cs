using AutoMapper;
using Financia.Communication.Responses;
using Financia.Domain.Entities;
using Financia.Domain.Repositories.Expenses;
using Financia.Exception;
using Financia.Exception.ExceptionBase.Exceptions;

namespace Financia.Application.UseCases.Expenses.ById
{
    public class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesRepository;
        private readonly IMapper _mapper;

        public GetExpenseByIdUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
        }

        public async Task<ResponseExpenseJson> Execute(long id)
        {
            Expense? result = await _expensesRepository.GetById(id);

            if (result is null)
            {
                throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
            }
            return _mapper.Map<ResponseExpenseJson>(result);
        }
    }
}
