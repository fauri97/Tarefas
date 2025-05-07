using AutoMapper;
using Tarefa.Application.UseCases.Tasks.Create.Dto;
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Exceptions.ExceptionBase;
using Tarefa.Domain.Entities;

namespace Tarefa.Application.UseCases.Tasks.Create
{
    public class CreateTaskUseCase(
        ITaskWriteOnlyRepository writeOnlyRepository,
        IUnityOfWork unityOfWork,
        IMapper mapper) : ICreateTaskUseCase
    {
        private readonly ITaskWriteOnlyRepository _writeOnlyRepository = writeOnlyRepository;
        private readonly IUnityOfWork _unityOfWork = unityOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CreatedTaskDto> ExecuteAsync(CreateTaskDto request, long userId)
        {
            Validate(request);

            var task = _mapper.Map<TaskEntity>(request);
            task.Status = false;
            task.CreatedAt = DateTime.UtcNow;
            task.ExpectedDate = DateTime.SpecifyKind(task.ExpectedDate, DateTimeKind.Utc);
            task.UserId = userId;

            _writeOnlyRepository.Add(task);
            await _unityOfWork.SaveChangesAsync();

            return _mapper.Map<CreatedTaskDto>(task);
        }

        private static void Validate(CreateTaskDto request)
        {
            var validator = new CreateTaskValidator();
            var result = validator.Validate(request);

            if (!result.IsValid)
            {
                throw new BusinessValidationException(result.Errors.Select(e => e.ErrorMessage));
            }
        }
    }
}
