using Financia.Application.UseCases.Login.DoLogin;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Communication.Responses.Error;
using Microsoft.AspNetCore.Mvc;

namespace Financia.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] RequestLoginJson request)
        {
            var response = await useCase.Execute(request);
            return Ok(response);
        }
    }
}
