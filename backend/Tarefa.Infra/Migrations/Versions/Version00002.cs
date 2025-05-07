using FluentMigrator;

namespace Tarefa.Infra.Migrations.Versions
{
    [Migration(DatabaseVersions.TABLE_USERS, "Create table to save Users")]
    public class Version00002 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Email").AsString().NotNullable().Unique()
                .WithColumn("Password").AsString().NotNullable()
                .WithColumn("UserIdentifier").AsGuid().NotNullable();
        }
    }
}
