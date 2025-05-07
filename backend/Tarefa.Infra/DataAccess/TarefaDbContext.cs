using Microsoft.EntityFrameworkCore;
using Tarefa.Domain.Entities;

namespace Tarefa.Infra.DataAccess
{
    public class TarefaDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            ConfigureTask(modelBuilder);
            ConfigureUser(modelBuilder);
        }

        private static void ConfigureTask(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskEntity>().ToTable("Tasks");
            modelBuilder.Entity<TaskEntity>()    
                .HasOne(t => t.User)    
                .WithMany(u => u.Tasks)    
                .HasForeignKey(t => t.UserId);            
        }

        private static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
        }
    }
}
