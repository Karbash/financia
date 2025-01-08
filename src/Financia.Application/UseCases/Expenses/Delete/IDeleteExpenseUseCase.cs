namespace Financia.Application.UseCases.Expenses.Delete
{
    public interface IDeleteExpenseUseCase
    {
        public Task Execute(long id);
    }
}
