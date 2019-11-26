using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class MigrateUserCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO dbo.WowCharacters (Name, UserId, WowClassId, WowServerId) SELECT PrimaryCharacterName, UserId, 1, 1 FROM dbo.Members ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
