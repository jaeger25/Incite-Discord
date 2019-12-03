using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class RemoveMemberStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
