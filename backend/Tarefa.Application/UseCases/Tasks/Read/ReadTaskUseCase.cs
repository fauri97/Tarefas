using Tarefa.Application.UseCases.Tasks.Read.Dto;
using Tarefa.Domain.Repositories.Task;

namespace Tarefa.Application.UseCases.Tasks.Read
{
    public class ReadTaskUseCase(ITaskReadOnlyRepository repository) : IReadTaskUseCase
    {
        private readonly ITaskReadOnlyRepository _repository = repository;
        public async Task<List<DtoReadTasks>> GetTasks()
        {
            var tasks = await _repository.GetAll();

            return [.. tasks.Select(task => new DtoReadTasks
            {
                Id = task.Id,
                CreatedAt = task.CreatedAt,
                Description = task.Description,
                ExpectedDate = task.ExpectedDate,
                ClosedAt = task.ClosedAt,
                Status = task.Status
            })];
        }
    }
}
