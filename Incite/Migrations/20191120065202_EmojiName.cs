using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class EmojiName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmojiDiscordId",
                table: "EventMembers");

            migrationBuilder.AddColumn<string>(
                name: "EmojiDiscordName",
                table: "EventMembers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmojiDiscordName",
                table: "EventMembers");

            migrationBuilder.AddColumn<decimal>(
                name: "EmojiDiscordId",
                table: "EventMembers",
                type: "decimal(20,0)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
