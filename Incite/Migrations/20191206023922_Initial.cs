using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Incite.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "WowServers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowServers", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Guilds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false),
                    WowServerId = table.Column<int>(nullable: true),
                    WowFaction = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Guilds", x => x.Id);
                    table.UniqueConstraint("AK_Guilds_DiscordId", x => x.DiscordId);
                    table.ForeignKey(
                        name: "FK_Guilds_WowServers_WowServerId",
                        column: x => x.WowServerId,
                        principalTable: "WowServers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WowItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowId = table.Column<int>(nullable: false),
                    ItemQuality = table.Column<int>(nullable: false),
                    WowHeadIcon = table.Column<string>(nullable: true),
                    WowItemClassId = table.Column<int>(nullable: false),
                    WowItemSubclassId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowItems_WowItemClasses_WowItemClassId",
                        column: x => x.WowItemClassId,
                        principalTable: "WowItemClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WowItems_WowItemSubclasses_WowItemSubclassId",
                        column: x => x.WowItemSubclassId,
                        principalTable: "WowItemSubclasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false),
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
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    DateTime = table.Column<DateTimeOffset>(nullable: false),
                    EventMessageId = table.Column<int>(nullable: false),
                    GuildId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Guilds_GuildId",
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
                name: "WowCharacters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowFaction = table.Column<int>(nullable: false),
                    GuildId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    WowClassId = table.Column<int>(nullable: false),
                    WowServerId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowCharacters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowCharacters_Guilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "Guilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateTable(
                name: "WowSpells",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    WowId = table.Column<int>(nullable: false),
                    CreatedItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WowSpells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WowSpells_WowItems_CreatedItemId",
                        column: x => x.CreatedItemId,
                        principalTable: "WowItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscordId = table.Column<decimal>(nullable: false),
                    ChannelId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrimaryWowCharacterId = table.Column<int>(nullable: true),
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
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_WowCharacters_PrimaryWowCharacterId",
                        column: x => x.PrimaryWowCharacterId,
                        principalTable: "WowCharacters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Members_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Events1",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    MessageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events1_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Events1_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventMembers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmojiDiscordName = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventMembers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MemberEvents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberEvents_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    { 12, "Skinning" },
                    { 11, "Mining" },
                    { 10, "Herbalism" },
                    { 9, "Tailoring" },
                    { 8, "Leatherworking" },
                    { 7, "Engineering" },
                    { 3, "Cooking" },
                    { 5, "Blacksmithing" },
                    { 4, "Alchemy" },
                    { 13, "Lockpicking" },
                    { 2, "Fishing" },
                    { 1, "FirstAid" },
                    { 6, "Enchanting" }
                });

            migrationBuilder.InsertData(
                table: "WowServers",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Kirtonos" });

            migrationBuilder.CreateIndex(
                name: "IX_Channels_GuildId",
                table: "Channels",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_EventMembers_EventId",
                table: "EventMembers",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventMembers_MemberId",
                table: "EventMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GuildId",
                table: "Events",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Events1_EventId",
                table: "Events1",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events1_MessageId",
                table: "Events1",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Guilds_WowServerId",
                table: "Guilds",
                column: "WowServerId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEvents_EventId",
                table: "MemberEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberEvents_MemberId",
                table: "MemberEvents",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_GuildId",
                table: "Members",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_PrimaryWowCharacterId",
                table: "Members",
                column: "PrimaryWowCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_UserId",
                table: "Members",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChannelId",
                table: "Messages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GuildId",
                table: "Roles",
                column: "GuildId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterProfessions_WowCharacterId",
                table: "WowCharacterProfessions",
                column: "WowCharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterProfessions_WowProfessionId",
                table: "WowCharacterProfessions",
                column: "WowProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterRecipes_RecipeId",
                table: "WowCharacterRecipes",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacterRecipes_WowCharacterProfessionId",
                table: "WowCharacterRecipes",
                column: "WowCharacterProfessionId");

            migrationBuilder.CreateIndex(
                name: "IX_WowCharacters_GuildId",
                table: "WowCharacters",
                column: "GuildId");

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

            migrationBuilder.CreateIndex(
                name: "IX_WowReagents_WowItemId",
                table: "WowReagents",
                column: "WowItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WowReagents_WowSpellId",
                table: "WowReagents",
                column: "WowSpellId");

            migrationBuilder.CreateIndex(
                name: "IX_WowSpells_CreatedItemId",
                table: "WowSpells",
                column: "CreatedItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMembers");

            migrationBuilder.DropTable(
                name: "Events1");

            migrationBuilder.DropTable(
                name: "MemberEvents");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "WowCharacterRecipes");

            migrationBuilder.DropTable(
                name: "WowReagents");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "WowCharacterProfessions");

            migrationBuilder.DropTable(
                name: "WowSpells");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "WowCharacters");

            migrationBuilder.DropTable(
                name: "WowProfessions");

            migrationBuilder.DropTable(
                name: "WowItems");

            migrationBuilder.DropTable(
                name: "Guilds");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WowClasses");

            migrationBuilder.DropTable(
                name: "WowItemSubclasses");

            migrationBuilder.DropTable(
                name: "WowServers");

            migrationBuilder.DropTable(
                name: "WowItemClasses");
        }
    }
}
