using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tarefa.Application.UseCases.Tasks.Read;

namespace Tarefa.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddUseCases(services);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IReadTaskUseCase, ReadTaskUseCase>();
        }
    }
}
