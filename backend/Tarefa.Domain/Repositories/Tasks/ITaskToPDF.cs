using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Tasks
{
    public interface ITaskToPDF
    {
        public byte[] GeneratePDF(ICollection<TaskEntity> tasks);
    }
}
