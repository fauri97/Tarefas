using Tarefa.Application.UseCases.Tasks.Update.Dto;

namespace Tarefa.Application.UseCases.Tasks.Update
{
    public interface IUpdateTaskUseCase
    {
        Task ExecuteAsync(UpdateTaskDto dto, long Id);
    }
}
