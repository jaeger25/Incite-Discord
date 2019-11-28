using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowItem3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WowItemId",
                table: "WowItems");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "WowItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WowId",
                table: "WowItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WowSpells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowSpells", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WowReagents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Count = table.Column<int>(nullable: false),
                    WowItemId = table.Column<int>(nullable: false),
                    WowSpellId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowReagents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowReagents_WowItems_WowItemId",
                        column: x => x.WowItemId,
                        principalTable: "WowItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowReagents_WowSpells_WowSpellId",
                        column: x => x.WowSpellId,
                        principalTable: "WowSpells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WowItems_CreatedById",
                table: "WowItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WowReagents_WowItemId",
                table: "WowReagents",
                column: "WowItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WowReagents_WowSpellId",
                table: "WowReagents",
                column: "WowSpellId");

            migrationBuilder.AddForeignKey(
                name: "FK_WowItems_WowSpells_CreatedById",
                table: "WowItems",
                column: "CreatedById",
                principalTable: "WowSpells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowItems_WowSpells_CreatedById",
                table: "WowItems");

            migrationBuilder.DropTable(
                name: "WowReagents");

            migrationBuilder.DropTable(
                name: "WowSpells");

            migrationBuilder.DropIndex(
                name: "IX_WowItems_CreatedById",
                table: "WowItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WowItems");

            migrationBuilder.DropColumn(
                name: "WowId",
                table: "WowItems");

            migrationBuilder.AddColumn<int>(
                name: "WowItemId",
                table: "WowItems",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
