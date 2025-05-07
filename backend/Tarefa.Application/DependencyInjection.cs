using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tarefa.Application.Services.AutoMapper;
using Tarefa.Application.Services.Cryptography;
using Tarefa.Application.UseCases.Tasks.Close;
using Tarefa.Application.UseCases.Tasks.Create;
using Tarefa.Application.UseCases.Tasks.Delete;
using Tarefa.Application.UseCases.Tasks.ExportToPDF;
using Tarefa.Application.UseCases.Tasks.Read;
using Tarefa.Application.UseCases.Tasks.Update;
using Tarefa.Application.UseCases.Users.Create;
using Tarefa.Application.UseCases.Users.Login;

namespace Tarefa.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
            AddCryptography(services);
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddScoped(option => new AutoMapper.MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper());
        }

        private static void AddCryptography(IServiceCollection services)
        {
            services.AddScoped<IPasswordService, PasswordService>();
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IReadTaskUseCase, ReadTaskUseCase>();
            services.AddScoped<ICreateTaskUseCase, CreateTaskUseCase>();
            services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();
            services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
            services.AddScoped<ICloseTaskUseCase, CloseTaskUseCase>();
            services.AddScoped<ITaskExportToPDFUseCase, TaskExportToPDFUseCase>();

            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();

            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
        }
    }
}
