using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Tasks.Delete
{
    public class DeleteTaskUseCase 
        (ITaskReadOnlyRepository taskReadOnly,
        ITaskDeleteOnlyRepository taskDeleteOnly,
        IUnityOfWork unityOfWork) : IDeleteTaskUseCase
    {
        private readonly ITaskReadOnlyRepository _taskReadOnly = taskReadOnly;
        private readonly ITaskDeleteOnlyRepository _taskDeleteOnly = taskDeleteOnly;
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        public async Task Delete(long id)
        {
            var task = await _taskReadOnly.GetByIdAsync(id)
                ?? throw new NotFoundException("Task não encontrada!");

            _taskDeleteOnly.Delete(task);
            await _unityOfWork.SaveChangesAsync();
        }
    }
}
