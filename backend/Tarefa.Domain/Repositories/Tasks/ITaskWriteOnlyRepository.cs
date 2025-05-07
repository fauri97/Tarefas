namespace Tarefa.Domain.Repositories.Tasks
{
    public interface ITaskWriteOnlyRepository
    {
        public void Add(Entities.TaskEntity task);
    }
}
