using Tarefa.Domain.Repositories;

namespace Tarefa.Infra.DataAccess
{
    public class UnityOfWork (TarefaDbContext context) : IUnityOfWork
    {
        private readonly TarefaDbContext _context = context;
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
