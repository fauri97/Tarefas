using Microsoft.EntityFrameworkCore;

namespace Tarefa.Infra.DataAccess
{
    public class TarefaDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Domain.Entities.TaskEntity> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureTask(modelBuilder);

        }

        private static void ConfigureTask(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Domain.Entities.TaskEntity>().ToTable("Tasks");
        }
    }
}
