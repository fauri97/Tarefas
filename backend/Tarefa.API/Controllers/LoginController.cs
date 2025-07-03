using Microsoft.AspNetCore.Mvc;
using Tarefa.API.Responses;
using Tarefa.Application.UseCases.Users.Login;
using Tarefa.Application.UseCases.Users.Login.Dto;

namespace Tarefa.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(
            [FromServices] IDoLoginUseCase useCase,
            [FromBody] DoLoginDto request)
        {
            var result = await useCase.Execute(request);

            ResponseBase response = new()
            {
                Message = "Login realizado com sucesso",
                StatusCode = StatusCodes.Status201Created,
                Data = result
            };
            return Created(string.Empty, response);
        }
    }
}
