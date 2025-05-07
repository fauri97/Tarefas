using System.Reflection;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tarefa.Domain.Repositories;
using Tarefa.Domain.Repositories.Tasks;
using Tarefa.Domain.Repositories.Users;
using Tarefa.Domain.Security.Tokens;
using Tarefa.Domain.Services.LoggedUser;
using Tarefa.Infra.DataAccess;
using Tarefa.Infra.DataAccess.PDF;
using Tarefa.Infra.DataAccess.Repositories;
using Tarefa.Infra.Extensions;
using Tarefa.Infra.Security.Tokens.Access;
using Tarefa.Infra.Services.LoggedUser;

namespace Tarefa.Infra
{
    public static class DependencyInjection
    {
        public static void AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContext(services, configuration);
            AddRepositories(services);
            AddFluentMigrator(services, configuration);
            AddTokens(services, configuration);
            AddLoggedUser(services);
        }

        private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();
            var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

            services.AddDbContext<TarefaDbContext>(options =>
            {
                options.UseNpgsql(connectionString);

                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<ITaskReadOnlyRepository, TasksRepository>();
            services.AddScoped<ITaskWriteOnlyRepository, TasksRepository>();
            services.AddScoped<ITaskUpdateOnlyRepository, TasksRepository>();
            services.AddScoped<ITaskDeleteOnlyRepository, TasksRepository>();
            services.AddScoped<ITaskToPDF, TaskToPDF>();

            services.AddScoped<IUserReadOnlyRepository, UsersRepository>();
            services.AddScoped<IUserWriteOnlyRepository, UsersRepository>();

            services.AddScoped<IUnityOfWork, UnityOfWork>();
        }

        private static void AddFluentMigrator(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(Assembly.Load("Tarefa.Infra")).For.All());
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(x => new JwtTokenGenerator(expirationTimeInMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(x => new JwtTokenValidator(signingKey!));
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();
    }
}
