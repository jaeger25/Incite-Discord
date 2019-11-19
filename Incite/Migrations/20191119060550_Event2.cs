using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class Event2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Messages_MessageId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "GuildId",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MessageId1",
                table: "Events",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_MessageId1",
                table: "Events",
                column: "MessageId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Guilds_MessageId",
                table: "Events",
                column: "MessageId",
                principalTable: "Guilds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Messages_MessageId1",
                table: "Events",
                column: "MessageId1",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Guilds_MessageId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Messages_MessageId1",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_MessageId1",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "MessageId1",
                table: "Events");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Messages_MessageId",
                table: "Events",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
