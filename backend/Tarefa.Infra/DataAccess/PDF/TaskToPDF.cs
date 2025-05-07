using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Tasks;

namespace Tarefa.Infra.DataAccess.PDF
{
    public class TaskToPDF : ITaskToPDF
    {
        public byte[] GeneratePDF(ICollection<TaskEntity> tasks)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            var localTasks = tasks.Select(task => new
            {
                task.Description,
                Expected = task.ExpectedDate.ToLocalTime(),
                Closed = task.ClosedAt?.ToLocalTime(),
                task.Status
            }).ToList();

            var pdfBytes = Document.Create(doc =>
            {
                doc.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(col =>
                    {
                        col.Item().Text("Relatório de Tarefas").FontSize(18).Bold().AlignCenter();
                        col.Item().PaddingVertical(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Descrição").Bold();
                                header.Cell().Text("Data Esperada").Bold();
                                header.Cell().Text("Concluída em").Bold();
                                header.Cell().Text("Status").Bold();
                            });

                            foreach (var task in localTasks)
                            {
                                table.Cell().Text(task.Description);
                                table.Cell().Text(task.Expected.ToString("dd/MM/yyyy HH:mm"));
                                table.Cell().Text(task.Closed?.ToString("dd/MM/yyyy HH:mm") ?? "-");
                                table.Cell().Text(task.Status ? "Fechado" : "Aberto");
                            }
                        });
                    });
                });
            }).GeneratePdf();

            return pdfBytes;
        }
    }
}
