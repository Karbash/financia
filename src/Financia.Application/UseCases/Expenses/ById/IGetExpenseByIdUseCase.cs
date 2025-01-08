using Financia.Communication.Responses;

namespace Financia.Application.UseCases.Expenses.ById
{
    public interface IGetExpenseByIdUseCase
    {
        public Task<ResponseExpenseJson> Execute(long id);
    }
}
