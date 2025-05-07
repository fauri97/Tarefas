namespace Tarefa.Application.UseCases.Tasks.Create.Dto
{
    public class CreateTaskDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime ExpectedDate { get; set; }
    }
}