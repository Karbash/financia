﻿using Financia.Application.UseCases.Expenses.Reports.Excel;
using Financia.Application.UseCases.Expenses.Reports.Pdf;
using Financia.Domain.Enuns;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Financia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.ADMIN)]
    public class ReportController : ControllerBase
    {
        [HttpGet("excel")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetExcel(
            [FromQuery] DateOnly month,
            [FromServices] IGenerateExpensesReportExcelUseCase useCase
            )
        {
            byte[] file = await useCase.Execute(month);

            if (file.Length > 0)
            {
                return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
            }
            return NoContent();
        }

        [HttpGet("pdf")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPdf(
            [FromQuery] DateOnly month,
            [FromServices] IGenerateExpensesReportPdfUseCase useCase
            )
        {
            byte[] file = await useCase.Execute(month);

            if (file.Length > 0)
            {
                return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
            }
            return NoContent();
        }
    }
}
