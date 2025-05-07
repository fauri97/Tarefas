using AutoMapper;
using Bogus;
using Moq;
using Tarefa.Application.Services.Cryptography;
using Tarefa.Application.UseCases.Users.Create;
using Tarefa.Application.UseCases.Users.Create.Dto;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Users;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests;
public class CreateUserUseCaseTests
{
    private readonly Mock<IUserReadOnlyRepository> _readOnlyRepoMock;
    private readonly Mock<IUserWriteOnlyRepository> _writeOnlyRepoMock;
    private readonly Mock<IAccessTokenGenerator> _tokenGeneratorMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IPasswordService> _passwordServiceMock;
    private readonly Mock<IUnityOfWork> _uowMock;
    private readonly CreateUserUseCase _useCase;
    private readonly Faker _faker;

    public CreateUserUseCaseTests()
    {
        _readOnlyRepoMock = new Mock<IUserReadOnlyRepository>();
        _writeOnlyRepoMock = new Mock<IUserWriteOnlyRepository>();
        _tokenGeneratorMock = new Mock<IAccessTokenGenerator>();
        _mapperMock = new Mock<IMapper>();
        _passwordServiceMock = new Mock<IPasswordService>();
        _uowMock = new Mock<IUnityOfWork>();
        _useCase = new CreateUserUseCase(
            _readOnlyRepoMock.Object,
            _writeOnlyRepoMock.Object,
            _tokenGeneratorMock.Object,
            _mapperMock.Object,
            _passwordServiceMock.Object,
            _uowMock.Object);
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_Criar_Usuario_Quando_Dados_Validos()
    {
        var dto = new CreateUserDto
        {
            Name = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(6)
        };

        var user = new User
        {
            Id = 1,
            Name = dto.Name,
            Email = dto.Email,
            CreatedAt = DateTime.UtcNow
        };

        var resultDto = new CreatedUserDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name
        };

        _readOnlyRepoMock.Setup(r => r.ExistUserWithEmail(dto.Email)).ReturnsAsync(false);
        _mapperMock.Setup(m => m.Map<User>(dto)).Returns(user);
        _passwordServiceMock.Setup(p => p.Hash(dto.Password)).Returns("hashed_pwd");
        _mapperMock.Setup(m => m.Map<CreatedUserDto>(user)).Returns(resultDto);
        _tokenGeneratorMock.Setup(t => t.Generate(user)).Returns("TOKEN");

        var result = await _useCase.ExecuteAsync(dto);

        Assert.Equal(dto.Email, result.Email, ignoreCase: true);
        Assert.Equal(dto.Name, result.Name);
        Assert.Equal("TOKEN", result.AccessToken);
        _writeOnlyRepoMock.Verify(r => r.CreateUser(It.IsAny<User>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        _passwordServiceMock.Verify(p => p.Hash(dto.Password), Times.Once);
        _tokenGeneratorMock.Verify(t => t.Generate(user), Times.Once);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Email_Ja_Existe()
    {
        var dto = new CreateUserDto
        {
            Name = _faker.Name.FullName(),
            Email = _faker.Internet.Email(),
            Password = _faker.Internet.Password(6)
        };

        _readOnlyRepoMock.Setup(r => r.ExistUserWithEmail(dto.Email)).ReturnsAsync(true);

        var ex = await Assert.ThrowsAsync<BusinessValidationException>(() => _useCase.ExecuteAsync(dto));
        Assert.Contains("Já existe um usuário com esse email.", ex.Errors);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Dados_Invalidos()
    {
        var dto = new CreateUserDto
        {
            Name = "",
            Email = "invalido",
            Password = "123"
        };

        _readOnlyRepoMock.Setup(r => r.ExistUserWithEmail(dto.Email)).ReturnsAsync(false);

        var ex = await Assert.ThrowsAsync<BusinessValidationException>(() => _useCase.ExecuteAsync(dto));
        Assert.True(ex.Errors.Any());
    }
}
