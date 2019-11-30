using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowCharacterRecipe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WowCharacterRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WowCharacterProfessionId = table.Column<int>(nullable: false),
                    RecipeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowCharacterRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowCharacterRecipes_WowItems_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "WowItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowCharacterRecipes_WowCharacterProfessions_WowCharacterProfessionId",
                        column: x => x.WowCharacterProfessionId,
                        principalTable: "WowCharacterProfessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterRecipes_RecipeId",
                table: "WowCharacterRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterRecipes_WowCharacterProfessionId",
                table: "WowCharacterRecipes",
                column: "WowCharacterProfessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WowCharacterRecipes");
        }
    }
}
