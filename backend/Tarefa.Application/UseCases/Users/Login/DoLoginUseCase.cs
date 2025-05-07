using Tarefa.Application.Services.Cryptography;
using Tarefa.Application.UseCases.Users.Login.Dto;
using Tarefa.Domain.Repositories.Users;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Users.Login
{
    public class DoLoginUseCase(
        IUserReadOnlyRepository userReadOnlyRepository,
        IAccessTokenGenerator accessTokenGenerator,
        IPasswordService passwordService) : IDoLoginUseCase
    {
        private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
        private readonly IAccessTokenGenerator _accessTokenGenerator = accessTokenGenerator;
        private readonly IPasswordService _passwordService = passwordService;
        public async Task<ResponseLoginDto> Execute(DoLoginDto dto)
        {
            var user = await _userReadOnlyRepository.GetByEmail(dto.Email)
            ?? throw new LoginException("Usuário ou senha incorreto, tente novamente");

            var verifiedPassword = _passwordService.Verify(dto.Password, user.Password);

            if (!verifiedPassword)
                throw new LoginException("Usuário ou senha incorreto, tente novamente");

            return new ResponseLoginDto
            {
                Email = dto.Email,
                Name = user.Name,
                AccessToken = _accessTokenGenerator.Generate(user),
                CreatedAt = user.CreatedAt
            };
        }
    }
}
