using Bogus;
using Moq;
using Tarefa.Application.UseCases.Tasks.Delete;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Domain.Repositories;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests
{
    public class DeleteTaskUseCaseTests
    {
        private readonly Mock<ITaskReadOnlyRepository> _readOnlyRepoMock;
        private readonly Mock<ITaskDeleteOnlyRepository> _deleteOnlyRepoMock;
        private readonly Mock<IUnityOfWork> _uowMock;
        private readonly DeleteTaskUseCase _useCase;
        private readonly Faker _faker;

        public DeleteTaskUseCaseTests()
        {
            _readOnlyRepoMock = new Mock<ITaskReadOnlyRepository>();
            _deleteOnlyRepoMock = new Mock<ITaskDeleteOnlyRepository>();
            _uowMock = new Mock<IUnityOfWork>();

            _useCase = new DeleteTaskUseCase(
                _readOnlyRepoMock.Object,
                _deleteOnlyRepoMock.Object,
                _uowMock.Object);

            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Deve_Deletar_Tarefa_Quando_Existe()
        {
            var taskId = _faker.Random.Long(1);
            var task = new TaskEntity { Id = taskId };

            _readOnlyRepoMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);

            await _useCase.Delete(taskId);

            _deleteOnlyRepoMock.Verify(d => d.Delete(task), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Tarefa_Nao_Existe()
        {
            var taskId = _faker.Random.Long(1);
            _readOnlyRepoMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync((TaskEntity?)null);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Delete(taskId));
            Assert.Equal("Task não encontrada!", ex.Message);
        }
    }
}
