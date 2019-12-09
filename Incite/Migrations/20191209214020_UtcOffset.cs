using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class UtcOffset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "UtcOffset",
                table: "WowServers",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 1,
                column: "UtcOffset",
                value: new TimeSpan(0, -5, 0, 0, 0));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UtcOffset",
                table: "WowServers");
        }
    }
}
