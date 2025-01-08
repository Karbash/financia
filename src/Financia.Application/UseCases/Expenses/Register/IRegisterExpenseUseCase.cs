using Financia.Communication.Requests;
using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Expenses.Register
{
    public interface IRegisterExpenseUseCase
    {
        public Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request);
    }
}
