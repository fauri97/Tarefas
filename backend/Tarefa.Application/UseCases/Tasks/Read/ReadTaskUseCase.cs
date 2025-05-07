using AutoMapper;
using Tarefa.Application.UseCases.Tasks.Read.Dto;
using Tarefa.Domain.Repositories.Tasks;

namespace Tarefa.Application.UseCases.Tasks.Read
{
    public class ReadTaskUseCase(ITaskReadOnlyRepository repository, IMapper mapper) : IReadTaskUseCase
    {
        private readonly ITaskReadOnlyRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        public async Task<List<DtoReadTasks>> GetTasks(long userId)
        {
            var tasks = await _repository.GetAll(userId);

            return [.. tasks.Select(task => _mapper.Map<DtoReadTasks>(task))];
        }
    }
}
