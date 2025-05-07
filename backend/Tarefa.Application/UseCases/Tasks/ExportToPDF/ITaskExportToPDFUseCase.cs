namespace Tarefa.Application.UseCases.Tasks.ExportToPDF
{
    public interface ITaskExportToPDFUseCase
    {
        Task<byte[]> ExportByIDS(List<long> ids);
    }
}
