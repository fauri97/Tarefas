using AutoMapper;
using Tarefa.Application.UseCases.Tasks.Update.Dto;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Domain.Repositories;
using Tarefa.Exceptions.ExceptionBase;

namespace Tarefa.Application.UseCases.Tasks.Update
{
    public class UpdateTaskUseCase(
    ITaskUpdateOnlyRepository repository,
    ITaskReadOnlyRepository readRepository,
    IUnityOfWork unityOfWork) : IUpdateTaskUseCase
    {
        public async Task ExecuteAsync(UpdateTaskDto dto, long Id)
        {
            Validate(dto);

            var existing = await readRepository.GetByIdAsync(Id) ?? throw new NotFoundException($"Tarefa com ID {Id} não encontrada.");

            if (dto.Description != existing.Description && !string.IsNullOrEmpty(dto.Description)) existing.Description = dto.Description;
            if (dto.Status != existing.Status && dto.Status != null) existing.Status = dto.Status.Value;
            if (dto.ExpectedDate != existing.ExpectedDate && dto.ExpectedDate != null) existing.ExpectedDate = dto.ExpectedDate.Value;

            existing.UpdatedAt = DateTime.UtcNow;

            repository.Update(existing);
            await unityOfWork.SaveChangesAsync();
        }

        private static void Validate(UpdateTaskDto dto)
        {
            var validator = new UpdateTaskValidator();
            var result = validator.Validate(dto);
            if (!result.IsValid)
                throw new BusinessValidationException(result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
