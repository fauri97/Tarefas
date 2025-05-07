namespace Tarefa.Domain.Security.Tokens
{
    public interface IAccessTokenValidator
    {
        public Guid ValidadeAndGetUserIdentifier(string token);
    }
}
