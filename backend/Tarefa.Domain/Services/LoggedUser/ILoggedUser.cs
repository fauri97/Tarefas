using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Services.LoggedUser
{
    public interface ILoggedUser
    {
        public Task<User> User();
    }
}
