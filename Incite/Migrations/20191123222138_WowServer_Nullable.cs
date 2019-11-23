using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowServer_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WowServerId",
                table: "Guilds",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WowServers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowServers", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "WowServers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Kirtonos" });

            migrationBuilder.Sql("UPDATE dbo.Guilds SET WowServerId = 1 WHERE WowServerId IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_WowServerId",
                table: "Guilds",
                column: "WowServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds",
                column: "WowServerId",
                principalTable: "WowServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds");

            migrationBuilder.DropTable(
                name: "WowServers");

            migrationBuilder.DropIndex(
                name: "IX_Guilds_WowServerId",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "WowServerId",
                table: "Guilds");
        }
    }
}
