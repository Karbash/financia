using Financia.Application.UseCases.Expenses.ById;
using Financia.Application.UseCases.Expenses.Delete;
using Financia.Application.UseCases.Expenses.GetAll;
using Financia.Application.UseCases.Expenses.Register;
using Financia.Application.UseCases.Expenses.Update;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Communication.Responses.Error;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Financia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterExpenseJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register
            ([FromBody] RequestExpenseJson request,
            [FromServices] IRegisterExpenseUseCase useCase)
        {
            ResponseRegisterExpenseJson response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ResponseExpensesJson), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllExpenses(
                            [FromServices] IGetAllExpensesUseCase useCase,
                            [FromQuery] int page = 1)
        {
            var response = await useCase.Execute(page);

            if (response.Expenses.Count != 0)
            {
                return Ok(response);
            }

            return NoContent();
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(ResponseExpenseJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetExpenseById(
            [FromServices] IGetExpenseByIdUseCase useCase, 
            [FromRoute] long id)
        {
            var response = await useCase.Execute(id);

            return Ok(response);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteById(
            [FromServices] IDeleteExpenseUseCase useCase,
            [FromRoute] long id)
        {
            await useCase.Execute(id);

            return NoContent();
        }

        [HttpPut("update/{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromServices] IUpdateExpenseUseCase useCase,
            [FromRoute] long id,
            [FromBody] RequestExpenseJson request)
        {
            await useCase.Execute(id, request);

            return NoContent();
        }
    }
}
