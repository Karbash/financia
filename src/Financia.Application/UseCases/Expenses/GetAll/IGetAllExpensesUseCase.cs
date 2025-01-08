using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Expenses.GetAll
{
    public interface IGetAllExpensesUseCase
    {
        public Task<ResponseExpensesJson> Execute(int page);
    }
}
