using Tarefa.Application.UseCases.Tasks.Read.Dto;

namespace Tarefa.Application.UseCases.Tasks.Read
{
    public interface IReadTaskUseCase
    {
        public Task<List<DtoReadTasks>> GetTasks();
    }
}
