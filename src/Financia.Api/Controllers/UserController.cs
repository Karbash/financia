using Financia.Application.UseCases.Users.Delete;
using Financia.Application.UseCases.Users.GetProfile;
using Financia.Application.UseCases.Users.PasswordChange;
using Financia.Application.UseCases.Users.Register;
using Financia.Application.UseCases.Users.UpdateProfile;
using Financia.Communication.Requests;
using Financia.Communication.Responses;
using Financia.Communication.Responses.Error;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("profile")]
        [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetProfile
            ([FromServices] IGetProfileUseCase useCase)
        {
            var response = await useCase.Execute();
            return Ok(response);
        }

        [HttpPut("update")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile
           ([FromBody] RequestUpdateUserJson request,
           [FromServices] IUpdateUserUseCase useCase)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpPut("password/change")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PasswordChange
           ([FromBody] RequestChangePasswordJson request,
           [FromServices] IChangePasswordUseCase useCase)
        {
            await useCase.Execute(request);
            return NoContent();
        }

        [HttpDelete("account/delete")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProfile
            ([FromServices] IDeleteUserAccountUseCase useCase)
        {
            await useCase.Execute();
            return NoContent();
        }
    }
}
