using Microsoft.EntityFrameworkCore;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Tasks;

namespace Tarefa.Infra.DataAccess.Repositories
{
    public class TasksRepository (TarefaDbContext dbContext) :
        ITaskReadOnlyRepository,
        ITaskWriteOnlyRepository,
        ITaskUpdateOnlyRepository,
        ITaskDeleteOnlyRepository

    {
        private readonly TarefaDbContext _dbContext = dbContext;

        public void Add(TaskEntity task)
            => _dbContext.Tasks.Add(task);

        public void Delete(TaskEntity task)
            => _dbContext.Tasks.Remove(task);

        public async Task<List<TaskEntity>> GetAll(long userId)    
            => await _dbContext.Tasks        
            .Where(t => t.UserId == userId)        
            .ToListAsync();


        public async Task<TaskEntity?> GetByIdAsync(long id)
            => await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        public async Task<ICollection<TaskEntity>> GetByRangeIdsAsync(List<long> ids)
            => await _dbContext.Tasks
                .Where(t => ids.Contains(t.Id))
                .ToListAsync();

        public void Update(TaskEntity task)
            => _dbContext.Tasks.Update(task);
    }
}
