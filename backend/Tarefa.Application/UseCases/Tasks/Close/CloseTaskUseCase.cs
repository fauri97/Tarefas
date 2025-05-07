
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Tasks.Close
{
    public class CloseTaskUseCase(
        ITaskUpdateOnlyRepository taskUpdateOnlyRepository,
        ITaskReadOnlyRepository taskReadOnlyRepository,
        IUnityOfWork unityOfWork) : ICloseTaskUseCase
    {
        private readonly ITaskUpdateOnlyRepository _taskUpdateOnlyRepository = taskUpdateOnlyRepository;
        private readonly ITaskReadOnlyRepository _taskReadOnlyRepository = taskReadOnlyRepository;
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        public async Task Close(long Id)
        {
            var task = await _taskReadOnlyRepository.GetByIdAsync(Id)
                ?? throw new NotFoundException("Tarefa não encontrada");

            task.Status = true;
            task.ClosedAt = DateTime.UtcNow;

            _taskUpdateOnlyRepository.Update(task);
            await _unityOfWork.SaveChangesAsync();
        }
    }
}
