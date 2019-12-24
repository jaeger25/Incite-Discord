using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class WowServers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WowServers",
                columns: new[] { "Id", "Name", "UtcOffset" },
                values: new object[,]
                {
                    { 2, "Ashkandi", new TimeSpan(0, -5, 0, 0, 0) },
                    { 64, "Finkle", new TimeSpan(0, 1, 0, 0, 0) },
                    { 63, "Sulfuron", new TimeSpan(0, 1, 0, 0, 0) },
                    { 62, "Amnennar", new TimeSpan(0, 1, 0, 0, 0) },
                    { 61, "Ten Storms", new TimeSpan(0, 1, 0, 0, 0) },
                    { 60, "Stonespine", new TimeSpan(0, 1, 0, 0, 0) },
                    { 59, "Skullflame", new TimeSpan(0, 1, 0, 0, 0) },
                    { 58, "Shazzrah", new TimeSpan(0, 1, 0, 0, 0) },
                    { 57, "Razorgore", new TimeSpan(0, 1, 0, 0, 0) },
                    { 65, "Heartstriker", new TimeSpan(0, 1, 0, 0, 0) },
                    { 56, "Noggenfogger", new TimeSpan(0, 1, 0, 0, 0) },
                    { 54, "Judgement", new TimeSpan(0, 1, 0, 0, 0) },
                    { 53, "Golemagg", new TimeSpan(0, 1, 0, 0, 0) },
                    { 52, "Gehennas", new TimeSpan(0, 1, 0, 0, 0) },
                    { 51, "Gandling", new TimeSpan(0, 1, 0, 0, 0) },
                    { 50, "Flamelash", new TimeSpan(0, 1, 0, 0, 0) },
                    { 49, "Firemaw", new TimeSpan(0, 1, 0, 0, 0) },
                    { 48, "Earthshaker", new TimeSpan(0, 1, 0, 0, 0) },
                    { 47, "Dreadmist", new TimeSpan(0, 1, 0, 0, 0) },
                    { 55, "Mograine", new TimeSpan(0, 1, 0, 0, 0) },
                    { 46, "Bloodfang", new TimeSpan(0, 1, 0, 0, 0) },
                    { 66, "Lucifron", new TimeSpan(0, 1, 0, 0, 0) },
                    { 68, "Patchwerk", new TimeSpan(0, 1, 0, 0, 0) },
                    { 86, "Grobbulus", new TimeSpan(0, -8, 0, 0, 0) },
                    { 85, "Deviate Delight", new TimeSpan(0, -5, 0, 0, 0) },
                    { 84, "Bloodsail Buccaneers", new TimeSpan(0, -5, 0, 0, 0) },
                    { 83, "Ivas", new TimeSpan(0, 11, 0, 0, 0) },
                    { 82, "Ragnaros", new TimeSpan(0, 11, 0, 0, 0) },
                    { 81, "Lokholar", new TimeSpan(0, 11, 0, 0, 0) },
                    { 80, "Iceblood", new TimeSpan(0, 11, 0, 0, 0) },
                    { 79, "Hillsbrad", new TimeSpan(0, 11, 0, 0, 0) },
                    { 67, "Venoxis", new TimeSpan(0, 1, 0, 0, 0) },
                    { 78, "Yojamba", new TimeSpan(0, 11, 0, 0, 0) },
                    { 76, "Arugal", new TimeSpan(0, 11, 0, 0, 0) },
                    { 75, "Mandokir", new TimeSpan(0, 1, 0, 0, 0) },
                    { 74, "Rhok’delar", new TimeSpan(0, 1, 0, 0, 0) },
                    { 73, "Wyrmthalak", new TimeSpan(0, 1, 0, 0, 0) },
                    { 72, "Flamegor", new TimeSpan(0, 1, 0, 0, 0) },
                    { 71, "Harbinger of Doom", new TimeSpan(0, 1, 0, 0, 0) },
                    { 70, "Transcendence", new TimeSpan(0, 1, 0, 0, 0) },
                    { 69, "Dragon's Call", new TimeSpan(0, 1, 0, 0, 0) },
                    { 77, "Felstriker", new TimeSpan(0, 11, 0, 0, 0) },
                    { 87, "Hydraxian Waterlords", new TimeSpan(0, 1, 0, 0, 0) },
                    { 45, "Ashbringer", new TimeSpan(0, 1, 0, 0, 0) },
                    { 43, "Thunderfury", new TimeSpan(0, -8, 0, 0, 0) },
                    { 20, "Anathema", new TimeSpan(0, -8, 0, 0, 0) },
                    { 19, "Remulos", new TimeSpan(0, 11, 0, 0, 0) },
                    { 18, "Chromie", new TimeSpan(0, 1, 0, 0, 0) },
                    { 17, "Lakeshire", new TimeSpan(0, 1, 0, 0, 0) },
                    { 16, "Razorfen", new TimeSpan(0, 1, 0, 0, 0) },
                    { 15, "Everlook", new TimeSpan(0, 1, 0, 0, 0) },
                    { 14, "Auberdine", new TimeSpan(0, 1, 0, 0, 0) },
                    { 13, "Pyrewood Village", new TimeSpan(0, 1, 0, 0, 0) },
                    { 21, "Arcanite Reaper", new TimeSpan(0, -8, 0, 0, 0) },
                    { 12, "Nethergarde Keep", new TimeSpan(0, 1, 0, 0, 0) },
                    { 10, "Windseeker", new TimeSpan(0, -5, 0, 0, 0) },
                    { 9, "Westfall", new TimeSpan(0, -5, 0, 0, 0) },
                    { 8, "Pagle", new TimeSpan(0, -5, 0, 0, 0) },
                    { 7, "Old Blanchy", new TimeSpan(0, -8, 0, 0, 0) },
                    { 6, "Myzrael", new TimeSpan(0, -8, 0, 0, 0) },
                    { 5, "Mankrik", new TimeSpan(0, -5, 0, 0, 0) },
                    { 4, "Azuresong", new TimeSpan(0, -8, 0, 0, 0) },
                    { 3, "Atiesh", new TimeSpan(0, -8, 0, 0, 0) },
                    { 11, "Mirage Raceway", new TimeSpan(0, 1, 0, 0, 0) },
                    { 44, "Whitemane", new TimeSpan(0, -8, 0, 0, 0) },
                    { 22, "Bigglesworth", new TimeSpan(0, -8, 0, 0, 0) },
                    { 24, "Blaumeux", new TimeSpan(0, -8, 0, 0, 0) },
                    { 42, "Thalnos", new TimeSpan(0, -5, 0, 0, 0) },
                    { 41, "Sulfuras", new TimeSpan(0, -5, 0, 0, 0) },
                    { 40, "Sul'thraze", new TimeSpan(0, -8, 0, 0, 0) },
                    { 39, "Stalagg", new TimeSpan(0, -5, 0, 0, 0) },
                    { 38, "Smolderweb", new TimeSpan(0, -8, 0, 0, 0) },
                    { 37, "Skeram", new TimeSpan(0, -5, 0, 0, 0) },
                    { 36, "Rattlegore", new TimeSpan(0, -8, 0, 0, 0) },
                    { 35, "Netherwind", new TimeSpan(0, -5, 0, 0, 0) },
                    { 23, "Benediction", new TimeSpan(0, -5, 0, 0, 0) },
                    { 34, "Loatheb", new TimeSpan(0, -4, 0, 0, 0) },
                    { 32, "Kromcrush", new TimeSpan(0, -5, 0, 0, 0) },
                    { 31, "Kurinaxx", new TimeSpan(0, -8, 0, 0, 0) },
                    { 30, "Incendius", new TimeSpan(0, -5, 0, 0, 0) },
                    { 29, "Herod", new TimeSpan(0, -5, 0, 0, 0) },
                    { 28, "Heartseeker", new TimeSpan(0, -5, 0, 0, 0) },
                    { 27, "Fairbanks", new TimeSpan(0, -8, 0, 0, 0) },
                    { 26, "Faerlina", new TimeSpan(0, -5, 0, 0, 0) },
                    { 25, "Earthfury", new TimeSpan(0, -5, 0, 0, 0) },
                    { 33, "Kurinnaxx", new TimeSpan(0, -5, 0, 0, 0) },
                    { 88, "Zandalar Tribe", new TimeSpan(0, 1, 0, 0, 0) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 57);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 63);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 65);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 66);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 67);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 68);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 69);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 70);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 71);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 72);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 73);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 74);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 75);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 76);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 77);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 79);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 80);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 81);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 82);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 83);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 84);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 86);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 87);

            migrationBuilder.DeleteData(
                table: "WowServers",
                keyColumn: "Id",
                keyValue: 88);
        }
    }
}
