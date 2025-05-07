using AutoMapper;
using Tarefa.Application.UseCases.Tasks.Create.Dto;
using Tarefa.Application.UseCases.Tasks.Read.Dto;
using Tarefa.Application.UseCases.Users.Create.Dto;
using Tarefa.Domain.Entities;

namespace Tarefa.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            CreateMap<CreateTaskDto, TaskEntity>()    
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))                
                .ForMember(dest => dest.Id, opt => opt.Ignore())    
                .ForMember(dest => dest.Status, opt => opt.Ignore())    
                .ForMember(dest => dest.ClosedAt, opt => opt.Ignore());

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        private void DomainToResponse()
        {
            CreateMap<TaskEntity, CreatedTaskDto>();
            CreateMap<TaskEntity, DtoReadTasks>();
            CreateMap<User, CreatedUserDto>();
        }
    }
}
