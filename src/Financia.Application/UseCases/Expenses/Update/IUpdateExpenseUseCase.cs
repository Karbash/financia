using Financia.Communication.Requests;

namespace Financia.Application.UseCases.Expenses.Update
{
    public interface IUpdateExpenseUseCase
    {
        public Task Execute(long id, RequestExpenseJson request);
    }
}
