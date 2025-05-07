using Microsoft.AspNetCore.Mvc;
using Tarefa.API.Responses;
using Tarefa.Application.UseCases.Users.Create;
using Tarefa.Application.UseCases.Users.Create.Dto;

namespace Tarefa.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateUserAsync(
            [FromBody] CreateUserDto request,
            [FromServices] ICreateUserUseCase useCase)
        {
            var result = await useCase.ExecuteAsync(request);
            ResponseBase response = new()
            {
                StatusCode = 201,
                Message = "Usuário criado com sucesso!",
                Data = result
            };
            return Created(string.Empty, response);
        }
    }
}
