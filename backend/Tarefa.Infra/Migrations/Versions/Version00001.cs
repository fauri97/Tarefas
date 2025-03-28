using FluentMigrator;

namespace Tarefa.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_TASKS, "Create table to save Tasks")]
    public class Version00001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Tasks")
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("ExpectedDate").AsDateTime().NotNullable()
                .WithColumn("ClosedAt").AsDateTime().Nullable()
                .WithColumn("Status").AsBoolean().NotNullable().WithDefaultValue(false);
        }
    }
}
