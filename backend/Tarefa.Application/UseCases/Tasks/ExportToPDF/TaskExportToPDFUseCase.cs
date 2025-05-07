
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Tasks.ExportToPDF
{
    public class TaskExportToPDFUseCase(ITaskToPDF taskToPDF, ITaskReadOnlyRepository taskReadOnlyRepository) : ITaskExportToPDFUseCase
    {
        private readonly ITaskToPDF _taskToPDF = taskToPDF;
        private readonly ITaskReadOnlyRepository _taskReadOnlyRepository = taskReadOnlyRepository;
        public async Task<byte[]> ExportByIDS(List<long> ids)
        {
            var tasks = await _taskReadOnlyRepository.GetByRangeIdsAsync(ids);

            if (tasks == null || tasks.Count == 0)
            {
                throw new NotFoundException("Nenhuma tarefa encontrada com os ids informados.");
            }

            return _taskToPDF.GeneratePDF(tasks);
        }
    }
}
