namespace Tarefa.Domain.Repositories.Task
{
    public interface ITaskReadOnlyRepository
    {
        public Task<List<Entities.TaskEntity>> GetAll();
    }
}
