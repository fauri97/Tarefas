﻿namespace Tarefa.Application.UseCases.Users.Create.Dto
{
    public class CreatedUserDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
