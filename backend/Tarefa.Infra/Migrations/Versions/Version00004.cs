using FluentMigrator;

namespace Tarefa.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.CREATE_TABLE_CATEGORIA, "Cria tabela categoria")]
    public class Version00004 : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("Categorias")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Descricao").AsString(100).NotNullable()
                .WithColumn("CreatedAt").AsDateTime().NotNullable()
                .WithColumn("UpdatedAt").AsDateTime().Nullable();
        }
    }
}