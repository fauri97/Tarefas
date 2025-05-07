using FluentMigrator;

namespace Tarefa.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.ALTER_TABLE_TASKS, "Adds UserId Collumn to table tasks")]
    public class Version00003 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Alter.Table("Tasks")
                .AddColumn("UserId").AsInt64().Nullable()
                .ForeignKey("FK_Tasks_Users", "Users", "Id");
        }
    }
}
