using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Tasks
{
    public interface ITaskUpdateOnlyRepository
    {
        void Update(TaskEntity task);
    }
}
