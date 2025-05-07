namespace Tarefa.Application.UseCases.Tasks.Update.Dto
{
    public class UpdateTaskDto
    {
        public string? Description { get; set; } = string.Empty;
        public DateTime? ExpectedDate { get; set; }
        public bool? Status { get; set; }
    }
}
