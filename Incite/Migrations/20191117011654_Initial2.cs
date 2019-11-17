using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class Initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Guilds_GuildId",
                table: "Members");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Guilds_GuildId",
                table: "Members",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Guilds_GuildId",
                table: "Members");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Guilds_GuildId",
                table: "Members",
                column: "GuildId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
