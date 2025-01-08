using AutoMapper;
using Financia.Communication.Responses;
using Financia.Domain.Repositories.Expenses;

namespace Financia.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesRepository;
        private readonly IMapper _mapper;
        public GetAllExpensesUseCase(IExpensesReadOnlyRepository expensesRepository, IMapper mapper)
        {
            _expensesRepository = expensesRepository;
            _mapper = mapper;
        }

        public async Task<ResponseExpensesJson> Execute(int page)
        {
            const int pageSize = 10;
            var result = await _expensesRepository.GetAll(page, pageSize);


            return new ResponseExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }
    }
}
