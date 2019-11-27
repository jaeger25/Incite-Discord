using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowCharacters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters");

            migrationBuilder.DropColumn(
                name: "PrimaryCharacterName",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "GuildId",
                table: "WowCharacters",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrimaryWowCharacterId",
                table: "Members",
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

            migrationBuilder.AddForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_WowCharacters_PrimaryWowCharacterId",
                table: "Members");

            migrationBuilder.DropForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters");

            migrationBuilder.DropIndex(
                name: "IX_Members_PrimaryWowCharacterId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "PrimaryWowCharacterId",
                table: "Members");

            migrationBuilder.AlterColumn<int>(
                name: "GuildId",
                table: "WowCharacters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "PrimaryCharacterName",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WowCharacters_Guilds_GuildId",
                table: "WowCharacters",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
