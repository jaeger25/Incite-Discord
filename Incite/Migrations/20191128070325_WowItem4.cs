using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowItem4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WowItemClassId",
                table: "WowItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WowItemSubclassId",
                table: "WowItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "WowItemClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowItemClasses", x => x.Id);
                    table.UniqueConstraint("AK_WowItemClasses_WowId", x => x.WowId);
                });

            migrationBuilder.CreateTable(
                name: "WowItemSubclasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowId = table.Column<int>(nullable: false),
                    WowItemClassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowItemSubclasses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowItemSubclasses_WowItemClasses_WowItemClassId",
                        column: x => x.WowItemClassId,
                        principalTable: "WowItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WowItems_WowItemClassId",
                table: "WowItems",
                column: "WowItemClassId");

            migrationBuilder.CreateIndex(
                name: "IX_WowItems_WowItemSubclassId",
                table: "WowItems",
                column: "WowItemSubclassId");

            migrationBuilder.CreateIndex(
                name: "IX_WowItemSubclasses_WowItemClassId",
                table: "WowItemSubclasses",
                column: "WowItemClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_WowItems_WowItemClasses_WowItemClassId",
                table: "WowItems",
                column: "WowItemClassId",
                principalTable: "WowItemClasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WowItems_WowItemSubclasses_WowItemSubclassId",
                table: "WowItems",
                column: "WowItemSubclassId",
                principalTable: "WowItemSubclasses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WowItems_WowItemClasses_WowItemClassId",
                table: "WowItems");

            migrationBuilder.DropForeignKey(
                name: "FK_WowItems_WowItemSubclasses_WowItemSubclassId",
                table: "WowItems");

            migrationBuilder.DropTable(
                name: "WowItemSubclasses");

            migrationBuilder.DropTable(
                name: "WowItemClasses");

            migrationBuilder.DropIndex(
                name: "IX_WowItems_WowItemClassId",
                table: "WowItems");

            migrationBuilder.DropIndex(
                name: "IX_WowItems_WowItemSubclassId",
                table: "WowItems");

            migrationBuilder.DropColumn(
                name: "WowItemClassId",
                table: "WowItems");

            migrationBuilder.DropColumn(
                name: "WowItemSubclassId",
                table: "WowItems");
        }
    }
}
