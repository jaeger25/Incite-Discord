using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Incite.Migrations
{
    public partial class Epgp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EpgpSnapshots",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DateTime = table.Column<DateTimeOffset>(nullable: false),
                    GuildId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpgpSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpgpSnapshots_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EpgpStandings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EffortPoints = table.Column<int>(nullable: false),
                    GearPoints = table.Column<int>(nullable: false),
                    SnapshotId = table.Column<int>(nullable: false),
                    WowCharacterId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EpgpStandings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EpgpStandings_EpgpSnapshots_SnapshotId",
                        column: x => x.SnapshotId,
                        principalTable: "EpgpSnapshots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EpgpStandings_WowCharacters_WowCharacterId",
                        column: x => x.WowCharacterId,
                        principalTable: "WowCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EpgpSnapshots_GuildId",
                table: "EpgpSnapshots",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_EpgpStandings_SnapshotId",
                table: "EpgpStandings",
                column: "SnapshotId");

            migrationBuilder.CreateIndex(
                name: "IX_EpgpStandings_WowCharacterId",
                table: "EpgpStandings",
                column: "WowCharacterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EpgpStandings");

            migrationBuilder.DropTable(
                name: "EpgpSnapshots");
        }
    }
}
