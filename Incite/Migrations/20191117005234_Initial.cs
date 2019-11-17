using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.UniqueConstraint("AK_Guilds_DiscordId", x => x.DiscordId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_DiscordId", x => x.DiscordId);
                });

            migrationBuilder.CreateTable(
                name: "WowClasses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WowProfessions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowProfessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false),
                    Kind = table.Column<int>(nullable: false),
                    GuildId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Channels_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false),
                    GuildId = table.Column<int>(nullable: false),
                    Kind = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryCharacterName = table.Column<string>(nullable: true),
                    GuildId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Members_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberRoles_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "WowClasses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Warrior" },
                    { 2, "Rogue" },
                    { 3, "Hunter" },
                    { 4, "Mage" },
                    { 5, "Warlock" },
                    { 6, "Priest" },
                    { 7, "Druid" },
                    { 8, "Shaman" },
                    { 9, "Paladin" }
                });

            migrationBuilder.InsertData(
                table: "WowProfessions",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 11, "Mining" },
                    { 10, "Herbalism" },
                    { 9, "Tailoring" },
                    { 8, "Leatherworking" },
                    { 7, "Engineering" },
                    { 2, "Fishing" },
                    { 5, "Blacksmithing" },
                    { 4, "Alchemy" },
                    { 3, "Cooking" },
                    { 12, "Skinning" },
                    { 1, "First Aid" },
                    { 6, "Enchanting" },
                    { 13, "Lockpicking" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_GuildId",
                table: "Channels",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_MemberId",
                table: "MemberRoles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_RoleId",
                table: "MemberRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GuildId",
                table: "Members",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildId",
                table: "Roles",
                column: "GuildId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropTable(
                name: "WowClasses");

            migrationBuilder.DropTable(
                name: "WowProfessions");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Guilds");
        }
    }
}
