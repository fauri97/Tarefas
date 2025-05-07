using Microsoft.AspNetCore.Mvc;
using Tarefa.API.Responses;
using Tarefa.Application.UseCases.Tasks.Create;
using Tarefa.Application.UseCases.Tasks.Create.Dto;
using Tarefa.Application.UseCases.Tasks.Read;
using Tarefa.Application.UseCases.Tasks.Update.Dto;
using Tarefa.Application.UseCases.Tasks.Update;
using Tarefa.Application.UseCases.Tasks.Delete;
using Tarefa.API.Attributes;
using Tarefa.API.Extensions;
using Tarefa.Application.UseCases.Tasks.Close;
using Tarefa.Application.UseCases.Tasks.ExportToPDF;

namespace Tarefa.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AuthenticatedUser]
    public class TaskController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTaskAsync(
            [FromBody] CreateTaskDto request,
            [FromServices] ICreateTaskUseCase useCase)
        {
            var userId = User.GetUserId();
            var result = await useCase.ExecuteAsync(request, userId);
            ResponseBase response = new()
            {
                StatusCode = 201,
                Message = "Task criada com sucesso!",
                Data = result
            };
            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseBase), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllRolesAsync(
            [FromServices] IReadTaskUseCase useCase)
        {
            var userId = User.GetUserId();
            var result = await useCase.GetTasks(userId);
            ResponseBase response = new()
            {
                StatusCode = 200,
                Message = "Tasks encontradas com sucesso!",
                Data = result
            };
            return Ok(response);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] long id,
            [FromBody] UpdateTaskDto dto,
            [FromServices] IUpdateTaskUseCase useCase)
        {
            await useCase.ExecuteAsync(dto, id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id,
            [FromServices] IDeleteTaskUseCase useCase)
        {
            await useCase.Delete(id);
            return NoContent();
        }

        [HttpPut("Close/{Id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CloseTask([FromRoute] long Id,
            [FromServices] ICloseTaskUseCase useCase)
        {
            await useCase.Close(Id);
            return NoContent();
        }

        [HttpGet("export/pdf")]
        public async Task<IActionResult> GetMaintenancePdf(
            [FromQuery] List<long> ids,    
            [FromServices] ITaskExportToPDFUseCase useCase)       
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    Console.WriteLine("Nenhum ID foi fornecido.");
                    return BadRequest("Nenhum ID foi fornecido.");
                }

                var pdfBytes = await useCase.ExportByIDS(ids);

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    Console.WriteLine("PDF gerado está nulo ou vazio.");
                    return StatusCode(500, "Erro ao gerar o PDF.");
                }
                return File(pdfBytes, "application/pdf", $"Tasks-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERRO ao gerar o PDF:");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Erro interno ao gerar o PDF.");
            }
        }

    }
}
