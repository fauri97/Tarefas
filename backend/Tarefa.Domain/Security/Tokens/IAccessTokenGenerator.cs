using Tarefa.Domain.Entities;

namespace Tarefa.Domain.Security.Tokens
{
    public interface IAccessTokenGenerator
    {
        public string Generate(User user);
    }
}
