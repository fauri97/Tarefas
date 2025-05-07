using Tarefa.Application.UseCases.Tasks.Create.Dto;

namespace Tarefa.Application.UseCases.Tasks.Create
{
    public interface ICreateTaskUseCase
    {
        Task<CreatedTaskDto> ExecuteAsync(CreateTaskDto request, long userId);
    }
}
