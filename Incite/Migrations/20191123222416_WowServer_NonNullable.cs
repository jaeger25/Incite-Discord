using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowServer_NonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds");

            migrationBuilder.AlterColumn<int>(
                name: "WowServerId",
                table: "Guilds",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds",
                column: "WowServerId",
                principalTable: "WowServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds");

            migrationBuilder.AlterColumn<int>(
                name: "WowServerId",
                table: "Guilds",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Guilds_WowServers_WowServerId",
                table: "Guilds",
                column: "WowServerId",
                principalTable: "WowServers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
