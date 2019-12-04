using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class Guild_WowFaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WowFaction",
                table: "Guilds",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WowFaction",
                table: "Guilds");
        }
    }
}
