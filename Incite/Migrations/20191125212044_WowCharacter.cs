using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowCharacter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WowCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    WowClassId = table.Column<int>(nullable: false),
                    WowServerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowCharacters_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowCharacters_WowClasses_WowClassId",
                        column: x => x.WowClassId,
                        principalTable: "WowClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowCharacters_WowServers_WowServerId",
                        column: x => x.WowServerId,
                        principalTable: "WowServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacters_UserId",
                table: "WowCharacters",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacters_WowClassId",
                table: "WowCharacters",
                column: "WowClassId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacters_WowServerId",
                table: "WowCharacters",
                column: "WowServerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WowCharacters");
        }
    }
}
