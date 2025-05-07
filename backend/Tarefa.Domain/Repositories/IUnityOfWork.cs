namespace Tarefa.Domain.Repositories
{
    public interface IUnityOfWork
    {
        Task SaveChangesAsync();
    }
}