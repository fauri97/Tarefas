using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Users
{
    public interface IUserWriteOnlyRepository
    {
        public void CreateUser(User user);
    }
}
