using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Tarefa.Infra.Migrations.Versions
{
    public abstract class VersionBase : ForwardOnlyMigration
    {
        protected ICreateTableColumnOptionOrWithColumnSyntax CreateTable(string tableName)
        {
            return Create.Table(tableName)
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("CreatedAt").AsDateTime().NotNullable().WithDefaultValue(System.DateTime.UtcNow)
                .WithColumn("UpdatedAt").AsDateTime().Nullable()
                .WithColumn("DeletedAt").AsDateTime().Nullable();
        }
    }
}