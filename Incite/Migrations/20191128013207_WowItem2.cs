using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowItem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ItemQuality",
                table: "WowItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "WowHeadIcon",
                table: "WowItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ItemQuality",
                table: "WowItems");

            migrationBuilder.DropColumn(
                name: "WowHeadIcon",
                table: "WowItems");
        }
    }
}
