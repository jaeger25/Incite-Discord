using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowProfession1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WowCharacterProfessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WowCharacterId = table.Column<int>(nullable: false),
                    WowProfessionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowCharacterProfessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowCharacterProfessions_WowCharacters_WowCharacterId",
                        column: x => x.WowCharacterId,
                        principalTable: "WowCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowCharacterProfessions_WowProfessions_WowProfessionId",
                        column: x => x.WowProfessionId,
                        principalTable: "WowProfessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterProfessions_WowCharacterId",
                table: "WowCharacterProfessions",
                column: "WowCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterProfessions_WowProfessionId",
                table: "WowCharacterProfessions",
                column: "WowProfessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WowCharacterProfessions");
        }
    }
}
