using Microsoft.EntityFrameworkCore;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Users;

namespace Tarefa.Infra.DataAccess.Repositories
{
    public class UsersRepository(TarefaDbContext dbContext) :
        IUserReadOnlyRepository,
        IUserWriteOnlyRepository
    {
        private readonly TarefaDbContext _dbContext = dbContext;

        public void CreateUser(User user)
            => _dbContext.Users.Add(user);

        public async Task<bool> ExistUserWithEmail(string email)
            => await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());

        public async Task<User?> GetByEmail(string email) =>
            await _dbContext.Users        
            .AsNoTracking()        
            .FirstOrDefaultAsync(x => x.Email == email.ToLower());


        public async Task<User?> GetByUserIdentifier(Guid guid)
            => await _dbContext.Users.FirstOrDefaultAsync(x => x.UserIdentifier == guid);
    }
}
