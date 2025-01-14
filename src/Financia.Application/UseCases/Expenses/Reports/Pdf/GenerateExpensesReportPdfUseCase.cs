using System.Reflection;
using Financia.Domain.Reports;
using Financia.Domain.Repositories.Expenses;
using Financia.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using Financia.Application.UseCases.Expenses.Reports.Pdf.Colors;

using PdfSharp.Fonts;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using Font = MigraDoc.DocumentObjectModel.Font;
using Cell = MigraDoc.DocumentObjectModel.Tables.Cell;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;
using DocumentFormat.OpenXml.Spreadsheet;
using Color = MigraDoc.DocumentObjectModel.Color;
using Financia.Domain.Extensions;
using Financia.Domain.Services.LoggedUser;

namespace Financia.Application.UseCases.Expenses.Reports.Pdf
{
    public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
    {
        private const string CURRENCY_SYMBOL = "$";
        private const int HEIGHT_LINE_EXPENSE_TABLE = 20;
        private readonly IExpensesReadOnlyRepository _repository;
        private readonly ILoggedUser _loggedUser;

        public GenerateExpensesReportPdfUseCase(IExpensesReadOnlyRepository expensesReadOnlyRepository,
            ILoggedUser loggedUser)
        {
            _repository = expensesReadOnlyRepository;
            _loggedUser = loggedUser;
            GlobalFontSettings.FontResolver = new ExpenseReportFontResolver();
        }

        public async Task<byte[]> Execute(DateOnly month)
        {
            var loggedUser = await _loggedUser.Get();

            var expenses = await _repository.FilterByMonth(loggedUser, month);

            if (expenses.Count == 0)
            {
                return [];
            }

            var document= CreateDocument(loggedUser.Name, month);
            var page  = CreatePage(document);

            CreateHeaderWhithProfilePhoto(loggedUser.Name, page);
            decimal sumExpenses = expenses.Sum(expenses => expenses.Amount);
            CreateTotalSpentSection(page, sumExpenses, month);

            foreach (var expense in expenses)
            {
                var table = CreateExpenseTable(page);

                var row = table.AddRow();
                row.Height = HEIGHT_LINE_EXPENSE_TABLE;

                AddExpenseTitle(row.Cells[0], expense.Title);
                AddExpenseHeaderAmount(row.Cells[3]);

                row = table.AddRow();
                row.Height = HEIGHT_LINE_EXPENSE_TABLE;

                row.Cells[0].AddParagraph(expense.Date.ToString("D"));
                SetStyleBaseForExpenseInformations(row.Cells[0], ColorsHelper.GREEN_DARK);
                row.Cells[0].Format.LeftIndent = 15;

                row.Cells[1].AddParagraph(expense.Date.ToString("t"));
                SetStyleBaseForExpenseInformations(row.Cells[1], ColorsHelper.GREEN_DARK);

                row.Cells[2].AddParagraph(expense.Payment.PaymentTypeToString());
                SetStyleBaseForExpenseInformations(row.Cells[2], ColorsHelper.GREEN_DARK);

                row.Cells[3].AddParagraph($"-{expense.Amount:f2} {CURRENCY_SYMBOL}");
                SetStyleBaseForExpenseInformations(row.Cells[3], ColorsHelper.WHITE, 14);

                if(string.IsNullOrWhiteSpace(expense.Description) == false)
                {
                    var descriptionRow = table.AddRow();
                    descriptionRow.Height = HEIGHT_LINE_EXPENSE_TABLE;
                    descriptionRow.Cells[0].AddParagraph(expense.Description);
                    SetStyleBaseForExpenseInformations(descriptionRow.Cells[0], ColorsHelper.GREEN_LIGHT, 10);
                    descriptionRow.Cells[0].MergeRight = 2;
                    descriptionRow.Cells[0].Format.LeftIndent = 15;

                    row.Cells[3].MergeDown = 1;

                }

                AddWhiteSpace(table);
            }

            return RenderDocument(document);
        }

        private Document CreateDocument(string author, DateOnly month) 
        { 
            var document = new Document();
            document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
            document.Info.Author = author;

            var style = document.Styles["Normal"];
            style!.Font.Name = FontHelper.ROBOTO_REGULAR;

            return document;
        }

        private Section CreatePage(Document document) 
        {
            var section = document.AddSection();

            section.PageSetup = document.DefaultPageSetup.Clone();

            section.PageSetup.PageFormat = PageFormat.A4;
            //Margin`s
            section.PageSetup.LeftMargin = 40;
            section.PageSetup.RightMargin = 40;
            section.PageSetup.TopMargin = 60;
            section.PageSetup.BottomMargin = 60;

            return section;
        }

        private void CreateHeaderWhithProfilePhoto(string username, Section page)
        {
            var table = page.AddTable();
            table.AddColumn();
            table.AddColumn(300);

            var row = table.AddRow();
            var assembly = Assembly.GetExecutingAssembly();
            var directoryName = Path.GetDirectoryName(assembly.Location);
            var pathFile = Path.Combine(directoryName!, "Logo", "financia-logo.png");

            row.Cells[0].AddImage(pathFile);
            row.Cells[1].AddParagraph($"Hey, {username}");

            row.Cells[1].Format.Font = new Font { Name = FontHelper.ROBOTO_THIN, Size = 12 };
            row.Cells[1].VerticalAlignment = MigraDoc.DocumentObjectModel.Tables.VerticalAlignment.Top;

            row.Cells[1].Format.LeftIndent = "10pt"; // Define indentação à esquerda
        }

        private void CreateTotalSpentSection(Section page, decimal sumExpenses, DateOnly month )
        {
            var paragraph = page.AddParagraph();
            paragraph.Format.SpaceBefore = "30";
            paragraph.Format.SpaceAfter = "30";
            var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPEND_IN, month.ToString("Y"));

            paragraph.AddFormattedText(title,
                new Font
                {
                    Name = FontHelper.ROBOTO_REGULAR,
                    Size = 15
                });

            paragraph.AddLineBreak();

           

            paragraph.AddFormattedText($"{sumExpenses:f2} {CURRENCY_SYMBOL}", 
                new Font
            {
                    Name = FontHelper.ROBOTO_BLACK,
                    Size = 40
            });

            paragraph.AddLineBreak();
        }

        private byte[] RenderDocument(Document document)
        {
            var renderer = new PdfDocumentRenderer
            {
                Document = document,
            };

            renderer.RenderDocument();

            using var file = new MemoryStream();
            renderer.PdfDocument.Save(file);

            return file.ToArray();
        }

        private Table CreateExpenseTable(Section page)
        {
            var table = page.AddTable();

            table.AddColumn("200").Format.Alignment = ParagraphAlignment.Left;
            table.AddColumn("100").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("100").Format.Alignment = ParagraphAlignment.Center;
            table.AddColumn("140").Format.Alignment = ParagraphAlignment.Right;

            return table;
        }

        private void AddExpenseTitle( Cell cell, string expenseTitle )
        {
            cell.AddParagraph(expenseTitle);
            cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.BLACK };
            cell.Shading.Color = ColorsHelper.RED_LIGHT;
            cell.VerticalAlignment = VerticalAlignment.Center;
            cell.MergeRight = 2;
            cell.Format.LeftIndent = 15;
        }

        private void AddExpenseHeaderAmount(Cell cell)
        {
            cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
            cell.Format.Font = new Font { Name = FontHelper.ROBOTO_BLACK, Size = 14, Color = ColorsHelper.WHITE };
            cell.Shading.Color = ColorsHelper.RED_DARK;
            cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void SetStyleBaseForExpenseInformations(Cell cell, Color backgroundColor, int size = 12)
        {
           cell.Format.Font = new Font { Name = FontHelper.ROBOTO_REGULAR, Size = size, Color = ColorsHelper.BLACK };
           cell.Shading.Color = backgroundColor;
           cell.VerticalAlignment = VerticalAlignment.Center;
        }

        private void AddWhiteSpace(Table table)
        {
            var row = table.AddRow();
            row.Height = HEIGHT_LINE_EXPENSE_TABLE;
            row.Borders.Visible = false;
        }
    }
}
