using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Tasks
{
    public interface ITaskReadOnlyRepository
    {
        public Task<List<TaskEntity>> GetAll(long userId);
        public Task<TaskEntity?> GetByIdAsync(long id);
        public Task<ICollection<TaskEntity>> GetByRangeIdsAsync(List<long> ids);
    }
}
