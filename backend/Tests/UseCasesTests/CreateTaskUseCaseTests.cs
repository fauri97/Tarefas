using AutoMapper;
using Bogus;
using Moq;
using Tarefa.Application.UseCases.Tasks.Create;
using Tarefa.Application.UseCases.Tasks.Create.Dto;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests;

public class CreateTaskUseCaseTests
{
    private readonly Mock<ITaskWriteOnlyRepository> _repoMock;
    private readonly Mock<IUnityOfWork> _uowMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateTaskUseCase _useCase;
    private readonly Faker _faker;

    public CreateTaskUseCaseTests()
    {
        _repoMock = new Mock<ITaskWriteOnlyRepository>();
        _uowMock = new Mock<IUnityOfWork>();
        _mapperMock = new Mock<IMapper>();
        _useCase = new CreateTaskUseCase(_repoMock.Object, _uowMock.Object, _mapperMock.Object);
        _faker = new Faker("pt_BR");
    }

    [Fact]
    public async Task Deve_Criar_Tarefa_Valida()
    {
        var dto = new CreateTaskDto
        {
            Description = _faker.Lorem.Sentence(3),
            ExpectedDate = DateTime.UtcNow.AddDays(_faker.Random.Int(1, 10))
        };

        var entity = new TaskEntity
        {
            Id = _faker.Random.Long(1),
            Description = dto.Description,
            ExpectedDate = dto.ExpectedDate,
            CreatedAt = DateTime.UtcNow,
            Status = false,
            UserId = _faker.Random.Long(1)
        };

        _mapperMock.Setup(m => m.Map<TaskEntity>(dto)).Returns(entity);

        var resultDto = new CreatedTaskDto
        {
            Id = entity.Id,
            Description = dto.Description
        };

        _mapperMock.Setup(m => m.Map<CreatedTaskDto>(entity)).Returns(resultDto);

        var result = await _useCase.ExecuteAsync(dto, entity.UserId);

        Assert.Equal(resultDto.Id, result.Id);
        Assert.Equal(resultDto.Description, result.Description);
        _repoMock.Verify(r => r.Add(It.IsAny<TaskEntity>()), Times.Once);
        _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Descricao_Vazia()
    {
        var dto = new CreateTaskDto
        {
            Description = "",
            ExpectedDate = _faker.Date.Soon()
        };

        var ex = await Assert.ThrowsAsync<BusinessValidationException>(() => _useCase.ExecuteAsync(dto, _faker.Random.Long(1)));

        Assert.Contains("Descrição é obrigatória.", ex.Errors);
        Assert.Contains("Descrição deve ter pelo menos 3 caracteres.", ex.Errors);
    }

    [Fact]
    public async Task Deve_Lancar_Excecao_Quando_Data_Esperada_No_Passado()
    {
        var dto = new CreateTaskDto
        {
            Description = _faker.Lorem.Sentence(2),
            ExpectedDate = DateTime.UtcNow.AddDays(-_faker.Random.Int(1, 10))
        };

        var ex = await Assert.ThrowsAsync<BusinessValidationException>(() => _useCase.ExecuteAsync(dto, _faker.Random.Long(1)));

        Assert.Contains(ex.Errors, msg => msg.StartsWith("Data esperada deve ser"));
    }

    [Fact]
    public async Task Deve_Setar_Status_Inicial_Como_False()
    {
        var dto = new CreateTaskDto
        {
            Description = _faker.Lorem.Sentence(),
            ExpectedDate = _faker.Date.Soon()
        };

        var entity = new TaskEntity
        {
            Description = dto.Description,
            ExpectedDate = dto.ExpectedDate
        };

        _mapperMock.Setup(m => m.Map<TaskEntity>(dto)).Returns(entity);
        _mapperMock.Setup(m => m.Map<CreatedTaskDto>(It.IsAny<TaskEntity>())).Returns(new CreatedTaskDto());

        await _useCase.ExecuteAsync(dto, _faker.Random.Long(1));

        Assert.False(entity.Status);
    }
}