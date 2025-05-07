using Tarefa.Application.UseCases.Users.Login.Dto;

namespace Tarefa.Application.UseCases.Users.Login
{
    public interface IDoLoginUseCase
    {
        public Task<ResponseLoginDto> Execute(DoLoginDto dto);
    }
}
