using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowCharacterGuild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuildId",
                table: "WowCharacters",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacters_GuildId",
                table: "WowCharacters",
                column: "GuildId");

            migrationBuilder.AddForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters");

            migrationBuilder.DropIndex(
                name: "IX_WowCharacters_GuildId",
                table: "WowCharacters");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "WowCharacters");
        }
    }
}
