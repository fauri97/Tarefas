namespace Tarefa.Application.UseCases.Tasks.Read.Dto
{
    public class DtoReadTasks
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpectedDate { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool Status { get; set; }
    }
}
