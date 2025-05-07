namespace Tarefa.Domain.Entities
{
    public class TaskEntity : EntityBase
    {
        public string Description { get; set; } = string.Empty;
        public DateTime ExpectedDate { get; set; }
        public DateTime? ClosedAt { get; set; }
        public bool Status { get; set; } = false;


        public long UserId { get; set; }
        public User? User { get; set; }
    }
}
