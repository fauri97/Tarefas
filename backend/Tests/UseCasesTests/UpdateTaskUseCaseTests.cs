using Bogus;
using Moq;
using Tarefa.Application.UseCases.Tasks.Update.Dto;
using Tarefa.Application.UseCases.Tasks.Update;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Domain.Repositories;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests
{
    public class UpdateTaskUseCaseTests
    {
        private readonly Mock<ITaskReadOnlyRepository> _readRepoMock;
        private readonly Mock<ITaskUpdateOnlyRepository> _updateRepoMock;
        private readonly Mock<IUnityOfWork> _uowMock;
        private readonly UpdateTaskUseCase _useCase;
        private readonly Faker _faker;

        public UpdateTaskUseCaseTests()
        {
            _readRepoMock = new Mock<ITaskReadOnlyRepository>();
            _updateRepoMock = new Mock<ITaskUpdateOnlyRepository>();
            _uowMock = new Mock<IUnityOfWork>();
            _useCase = new UpdateTaskUseCase(_updateRepoMock.Object, _readRepoMock.Object, _uowMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Atualizar_Tarefa_Com_Dados_Validos()
        {
            var id = _faker.Random.Long(1);
            var dto = new UpdateTaskDto
            {
                Description = _faker.Lorem.Sentence(3),
                ExpectedDate = DateTime.UtcNow.AddDays(2),
                Status = true
            };

            var existing = new TaskEntity
            {
                Id = id,
                Description = "Antiga descrição",
                ExpectedDate = DateTime.UtcNow,
                Status = false,
                UpdatedAt = null
            };

            _readRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            await _useCase.ExecuteAsync(dto, id);

            Assert.Equal(dto.Description, existing.Description);
            Assert.Equal(dto.ExpectedDate.Value, existing.ExpectedDate);
            Assert.Equal(dto.Status.Value, existing.Status);
            Assert.NotNull(existing.UpdatedAt);
            _updateRepoMock.Verify(r => r.Update(existing), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Tarefa_Nao_Encontrada()
        {
            var id = _faker.Random.Long(1);
            var dto = new UpdateTaskDto
            {
                Description = _faker.Lorem.Sentence(3),
                ExpectedDate = DateTime.UtcNow.AddDays(2),
                Status = true
            };

            _readRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((TaskEntity?)null);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.ExecuteAsync(dto, id));
            Assert.Contains("Tarefa com ID", ex.Message);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Validacao_Falhar()
        {
            var id = _faker.Random.Long(1);
            var dto = new UpdateTaskDto
            {
                Description = "",
                Status = true
            };

            var ex = await Assert.ThrowsAsync<BusinessValidationException>(() => _useCase.ExecuteAsync(dto, id));
            Assert.True(ex.Errors.Count() > 0);
        }

        [Fact]
        public async Task Nao_Deve_Atualizar_Campos_Iguais_ou_Nulos()
        {
            var id = _faker.Random.Long(1);
            var dto = new UpdateTaskDto
            {
                Description = null,
                ExpectedDate = null,
                Status = null
            };

            var existing = new TaskEntity
            {
                Id = id,
                Description = "Descrição atual",
                ExpectedDate = DateTime.UtcNow,
                Status = false
            };

            _readRepoMock.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(existing);

            await _useCase.ExecuteAsync(dto, id);

            _updateRepoMock.Verify(r => r.Update(existing), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }
    }
}