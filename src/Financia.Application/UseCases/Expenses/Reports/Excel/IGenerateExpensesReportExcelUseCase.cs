namespace Financia.Application.UseCases.Expenses.Reports.Excel
{
    public interface IGenerateExpensesReportExcelUseCase
    {
        public Task<Byte[]> Execute(DateOnly month);
    }
}
