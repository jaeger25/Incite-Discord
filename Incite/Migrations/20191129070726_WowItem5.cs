using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowItem5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowItems_WowSpells_CreatedById",
                table: "WowItems");

            migrationBuilder.DropIndex(
                name: "IX_WowItems_CreatedById",
                table: "WowItems");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_WowItemClasses_WowId",
                table: "WowItemClasses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "WowItems");

            migrationBuilder.AddColumn<int>(
                name: "CreatedItemId",
                table: "WowSpells",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WowSpells_CreatedItemId",
                table: "WowSpells",
                column: "CreatedItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_WowSpells_WowItems_CreatedItemId",
                table: "WowSpells",
                column: "CreatedItemId",
                principalTable: "WowItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowSpells_WowItems_CreatedItemId",
                table: "WowSpells");

            migrationBuilder.DropIndex(
                name: "IX_WowSpells_CreatedItemId",
                table: "WowSpells");

            migrationBuilder.DropColumn(
                name: "CreatedItemId",
                table: "WowSpells");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "WowItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_WowItemClasses_WowId",
                table: "WowItemClasses",
                column: "WowId");

            migrationBuilder.CreateIndex(
                name: "IX_WowItems_CreatedById",
                table: "WowItems",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_WowItems_WowSpells_CreatedById",
                table: "WowItems",
                column: "CreatedById",
                principalTable: "WowSpells",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
