using Moq;
using Tarefa.Application.UseCases.Tasks.Close;
using Tarefa.Domain.Entities;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Domain.Repositories;
using Tarefa.Exceptions.ExceptionBase;

namespace UseCasesTests
{
    public class CloseTaskUseCaseTests
    {
        private readonly Mock<ITaskReadOnlyRepository> _readOnlyRepoMock;
        private readonly Mock<ITaskUpdateOnlyRepository> _updateRepoMock;
        private readonly Mock<IUnityOfWork> _uowMock;
        private readonly CloseTaskUseCase _useCase;

        public CloseTaskUseCaseTests()
        {
            _readOnlyRepoMock = new Mock<ITaskReadOnlyRepository>();
            _updateRepoMock = new Mock<ITaskUpdateOnlyRepository>();
            _uowMock = new Mock<IUnityOfWork>();

            _useCase = new CloseTaskUseCase(
                _updateRepoMock.Object,
                _readOnlyRepoMock.Object,
                _uowMock.Object);
        }

        [Fact]
        public async Task Deve_Fechar_Tarefa_Corretamente()
        {
            var task = new TaskEntity
            {
                Id = 1,
                Status = false,
                ClosedAt = null
            };

            _readOnlyRepoMock.Setup(r => r.GetByIdAsync(task.Id)).ReturnsAsync(task);

            await _useCase.Close(task.Id);

            Assert.True(task.Status);
            Assert.NotNull(task.ClosedAt);
            Assert.True(task.ClosedAt.Value <= DateTime.UtcNow);

            _updateRepoMock.Verify(r => r.Update(task), Times.Once);
            _uowMock.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Se_Tarefa_Nao_Existir()
        {
            _readOnlyRepoMock.Setup(r => r.GetByIdAsync(It.IsAny<long>())).ReturnsAsync((TaskEntity?)null);

            var ex = await Assert.ThrowsAsync<NotFoundException>(() => _useCase.Close(99));
            Assert.Equal("Tarefa não encontrada", ex.Message);
        }
    }
}
