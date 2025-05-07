using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Tasks
{
    public interface ITaskDeleteOnlyRepository
    {
        void Delete(TaskEntity task);
    }
}
