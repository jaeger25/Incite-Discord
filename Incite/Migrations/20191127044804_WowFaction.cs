using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowFaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WowFaction",
                table: "WowCharacters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "WowProfessions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "FirstAid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WowFaction",
                table: "WowCharacters");

            migrationBuilder.UpdateData(
                table: "WowProfessions",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "First Aid");
        }
    }
}
