using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Tarefa.Domain.Repositories.Task;

namespace Tarefa.Infra.DataAccess.Repositories
{
    public class TasksRepository (TarefaDbContext dbContext) : ITaskReadOnlyRepository

    {
        private readonly TarefaDbContext _dbContext = dbContext;
        public async Task<List<Domain.Entities.TaskEntity>> GetAll()
            => await _dbContext.Tasks.ToListAsync();
    }
}
