
namespace Financia.Application.UseCases.Expenses.Reports.Pdf
{
    public interface IGenerateExpensesReportPdfUseCase
    {
        public Task<Byte[]> Execute(DateOnly month);
    }
}
