using Tarefa.Application.UseCases.Users.Create.Dto;

namespace Tarefa.Application.UseCases.Users.Create
{
    public interface ICreateUserUseCase
    {
        Task<CreatedUserDto> ExecuteAsync(CreateUserDto createUserDto);
    }
}
