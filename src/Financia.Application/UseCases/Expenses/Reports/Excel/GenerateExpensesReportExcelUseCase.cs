﻿
using ClosedXML.Excel;
using Financia.Domain.Extensions;
using Financia.Domain.Reports;
using Financia.Domain.Repositories.Expenses;

namespace Financia.Application.UseCases.Expenses.Reports.Excel
{
    public class GenerateExpensesReportExcelUseCase : IGenerateExpensesReportExcelUseCase
    {
        private const string CURRENCY_SYMBOL = "$";
        private readonly IExpensesReadOnlyRepository _repository;

        public GenerateExpensesReportExcelUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository)
        {
            _repository = expensesReadOnlyRepository;
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var expenses = await _repository.FilterByMonth(month);

            if (expenses.Count == 0)
            {
                return Array.Empty<byte>();
            }

            using var workbook = new XLWorkbook
            {
                Author = "Financia Software"
            };

            workbook.Style.Font.FontSize = 12;
            workbook.Style.Font.FontName = "Times New Roman";

            var worksheet = workbook.Worksheets.Add(month.ToString("Y"));
            InsertHeader(worksheet);

            var raw = 2;
            foreach (var expense in expenses)
            {
                worksheet.Cell($"A{raw}").Value = expense.Title;
                worksheet.Cell($"B{raw}").Value = expense.Date;
                worksheet.Cell($"C{raw}").Value = expense.Payment.PaymentTypeToString();

                worksheet.Cell($"D{raw}").Value = expense.Amount;
                worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{CURRENCY_SYMBOL} #,##0.00";

                worksheet.Cell($"E{raw}").Value = expense.Description;

                raw++;
            }

            worksheet.Columns().AdjustToContents();

            var file = new MemoryStream();

            workbook.SaveAs(file);

            return file.ToArray();
        }


        private void InsertHeader(IXLWorksheet worksheet)
        {
            worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
            worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
            worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT;
            worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
            worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

            worksheet.Cells("A1:E1").Style.Font.Bold = true;
            worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");

            worksheet.Cell("A1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("B1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
    }
}
