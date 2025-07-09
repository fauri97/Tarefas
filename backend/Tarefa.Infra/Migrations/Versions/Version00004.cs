using FluentMigrator;

namespace Tarefa.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.CREATE_TABLE_CATEGORIES, "Create table categories")]
    public class Version00004 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Name").AsString(100).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }
    }
}
