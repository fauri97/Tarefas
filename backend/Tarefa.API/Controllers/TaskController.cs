using Microsoft.AspNetCore.Mvc;
using Tarefa.API.Responses;
using Tarefa.Application.UseCases.Tasks.Read;

namespace Tarefa.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllRolesAsync(
            [FromServices] IReadTaskUseCase useCase)
        {
            var result = await useCase.GetTasks();
            ResponseBase response = new ResponseBase
            {
                StatusCode = 200,
                Message = "Tasks found successfully",
                Data = result
            };
            return Ok(response);
        }
    }
}
