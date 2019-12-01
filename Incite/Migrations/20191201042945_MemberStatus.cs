using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class MemberStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Channels");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Members",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("UPDATE dbo.Members SET Status = 2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Channels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
