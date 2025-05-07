using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Repositories.Users
{
    public interface IUserReadOnlyRepository
    {
        Task<User?> GetByUserIdentifier (Guid guid);
        Task<User?> GetByEmail(string email);
        Task<bool> ExistUserWithEmail (string email);
    }
}
