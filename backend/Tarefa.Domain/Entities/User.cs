namespace Tarefa.Domain.Entities
{
    public class User : EntityBase
    {
        private string _email = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email
        {
            get => _email;
            set => _email = value.ToLower();
        }
        public string Password { get; set; } = string.Empty;
        public Guid UserIdentifier { get; set; }

        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}
