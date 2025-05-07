namespace Tarefa.Application.UseCases.Tasks.Create.Dto
{
    public class CreatedTaskDto
    {
        public long Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpectedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
