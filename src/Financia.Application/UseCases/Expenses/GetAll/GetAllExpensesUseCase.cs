using AutoMapper;
using Financia.Communication.Responses;
using Financia.Domain.Entities;
using Financia.Domain.Repositories.Expenses;
using Financia.Domain.Services.LoggedUser;

namespace Financia.Application.UseCases.Expenses.GetAll
{
    public class GetAllExpensesUseCase : IGetAllExpensesUseCase
    {
        private readonly IExpensesReadOnlyRepository _expensesRepository;
        private readonly IMapper _mapper;
        private readonly ILoggedUser _loggedUser;
        public GetAllExpensesUseCase(IExpensesReadOnlyRepository expensesRepository, ILoggedUser loggedUser, IMapper mapper)
        {
            _loggedUser = loggedUser;
            _expensesRepository = expensesRepository;
            _mapper = mapper;
        }

        public async Task<ResponseExpensesJson> Execute(int page)
        {
            User loggedUser = await _loggedUser.Get();

            const int pageSize = 10;
            var result = await _expensesRepository.GetAll(page, pageSize, loggedUser);


            return new ResponseExpensesJson
            {
                Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
            };
        }
    }
}
