using Financia.Application.UseCases.Users.Register;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Communication.Responses.Error;
using Microsoft.AspNetCore.Mvc;

namespace Financia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register
            ([FromBody] RequestRegisterUserJson request,
            [FromServices] IRegisterUserUseCase useCase)
        {
            var response = await useCase.Execute(request);
            return Created(string.Empty, response);
        }
    }
}
