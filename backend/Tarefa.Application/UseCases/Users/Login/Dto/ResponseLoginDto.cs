namespace Tarefa.Application.UseCases.Users.Login.Dto
{
    public class ResponseLoginDto
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string AccessToken { get; set; } = string.Empty;
    }
}
