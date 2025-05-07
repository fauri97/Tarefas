using Bogus;
using Moq;
using Tarefa.Application.Services.Cryptography;
using Tarefa.Application.UseCases.Users.Login;
using Tarefa.Application.UseCases.Users.Login.Dto;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Users;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests;

public class DoLoginUseCaseTests
{
    private readonly Mock<IUserReadOnlyRepository> _userRepoMock;
    private readonly Mock<IAccessTokenGenerator> _tokenGeneratorMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly DoLoginUseCase _useCase;
    private readonly Faker _faker;

    public DoLoginUseCaseTests()
    {
        _userRepoMock = new Mock<IUserReadOnlyRepository>();
        _tokenGeneratorMock = new Mock<IAccessTokenGenerator>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _useCase = new DoLoginUseCase(
            _userRepoMock.Object,
            _tokenGeneratorMock.Object,
            _passwordServiceMock.Object);

        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_Retornar_Token_Quando_Credenciais_Corretas()
    {
        var email = _faker.Internet.Email();
        var plainPassword = _faker.Internet.Password();
        var hashedPassword = "hash_fake";

        var dto = new DoLoginDto { Email = email, Password = plainPassword };

        var fakeUser = new User
        {
            Name = _faker.Name.FullName(),
            Email = email,
            Password = hashedPassword,
            CreatedAt = _faker.Date.Past()
        };

        _userRepoMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(fakeUser);
        _passwordServiceMock.Setup(p => p.Verify(plainPassword, hashedPassword)).Returns(true);
        _tokenGeneratorMock.Setup(t => t.Generate(fakeUser)).Returns("TOKEN_FAKE");

        var result = await _useCase.Execute(dto);

        Assert.Equal(email, result.Email);
        Assert.Equal(fakeUser.Name, result.Name);
        Assert.Equal("TOKEN_FAKE", result.AccessToken);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Email_Invalido()
    {
        var dto = new DoLoginDto
        {
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password()
        };

        _userRepoMock.Setup(r => r.GetByEmail(dto.Email)).ReturnsAsync((User?)null);

        var ex = await Assert.ThrowsAsync<LoginException>(() => _useCase.Execute(dto));
        Assert.Contains("Usuário ou senha", ex.Message);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Senha_Incorreta()
    {
        var email = _faker.Internet.Email();
        var plainPassword = _faker.Internet.Password();
        var wrongPassword = _faker.Internet.Password();

        var dto = new DoLoginDto { Email = email, Password = wrongPassword };

        var fakeUser = new User
        {
            Name = _faker.Name.FullName(),
            Email = email,
            Password = "hash_do_usuario",
            CreatedAt = _faker.Date.Past()
        };

        _userRepoMock.Setup(r => r.GetByEmail(email)).ReturnsAsync(fakeUser);
        _passwordServiceMock.Setup(p => p.Verify(wrongPassword, fakeUser.Password)).Returns(false);

        var ex = await Assert.ThrowsAsync<LoginException>(() => _useCase.Execute(dto));
        Assert.Contains("Usuário ou senha", ex.Message);
    }
}
