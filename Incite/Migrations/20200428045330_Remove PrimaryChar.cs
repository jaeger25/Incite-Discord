using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class RemovePrimaryChar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_WowCharacters_PrimaryWowCharacterId",
                table: "Members");

            migrationBuilder.DropIndex(
                name: "IX_Members_PrimaryWowCharacterId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PrimaryWowCharacterId",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PrimaryWowCharacterId",
                table: "Members",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Members_PrimaryWowCharacterId",
                table: "Members",
                column: "PrimaryWowCharacterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_WowCharacters_PrimaryWowCharacterId",
                table: "Members",
                column: "PrimaryWowCharacterId",
                principalTable: "WowCharacters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
