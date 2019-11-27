using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.ApiModels;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Incite.Discord.Services;

namespace Incite.Discord.Commands
{
    [Group("wow")]
    [Description("Commands for managing info related to in-game WoW concepts")]
    public class WowCommands : BaseCommandModule
    {
        [Group("character")]
        [Aliases("char")]
        [Description("Commands for managing your characters")]
        public class WowCharacterCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public WowCharacterCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("list")]
            [Description("Lists your characters")]
            public async Task List(CommandContext context)
            {
                StringBuilder characterList = new StringBuilder("__**Character**__ , __**Class**__");
                foreach (var character in User.WowCharacters.OrderBy(x => x.WowServerId))
                {
                    characterList.Append($"\n{character} , {character.WowClass}");
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(characterList.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("add")]
            [Description("Adds a character to your character list")]
            public async Task AddCharacter(CommandContext context,
                [Description(Descriptions.WowCharacter)] QualifiedWowCharacter character,
                [Description(Descriptions.WowClass)] WowClass characterClass,
                [Description(Descriptions.WowFaction)] WowFaction faction)
            {
                if (User.WowCharacters.Any(x => x.Name == character.Name && x.WowServerId == character.Server.Id))
                {
                    ResponseString = "You have already registered this character";
                    return;
                }

                var wowCharacter = new WowCharacter()
                {
                    Name = character.Name,
                    WowFaction = faction,
                    GuildId = Guild.Id,
                    UserId = User.Id,
                    WowClassId = characterClass.Id,
                    WowServerId = character.Server.Id,
                };

                User.WowCharacters.Add(wowCharacter);

                if (Member.PrimaryWowCharacter == null)
                {
                    Member.PrimaryWowCharacter = wowCharacter;
                }

                await m_dbContext.SaveChangesAsync();
            }

            [Command("remove")]
            [Description("Removes a character from your character list")]
            public async Task RemoveCharacter(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character)
            {
                m_dbContext.WowCharacters.Remove(character.Character);
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-class")]
            [Description("Sets a property for one of your characters")]
            public async Task SetClass(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowClass)] WowClass wowClass)
            {
                character.Character.WowClassId = wowClass.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-server")]
            [Description("Sets a property for one of your characters")]
            public async Task SetServer(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowServer)] WowServer server)
            {
                character.Character.WowServerId = server.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-faction")]
            [Description("Sets a property for one of your characters")]
            public async Task SetFaction(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowFaction)] WowFaction faction)
            {
                character.Character.WowFaction = faction;
                await m_dbContext.SaveChangesAsync();
            }
        }

        [Group("profession")]
        [Aliases("prof")]
        [Description("Commands for searching and managing professions")]
        public class WowProfessionCommands : BaseInciteCommand
        {
            readonly InciteDbContext m_dbContext;

            public WowProfessionCommands(InciteDbContext dbContext)
            {
                m_dbContext = dbContext;
            }

            [Command("list")]
            [Description("Lists your characters' profressions")]
            public async Task List(CommandContext context)
            {
                StringBuilder message = new StringBuilder();
                foreach (var character in User.WowCharacters)
                {
                    message.Append($"{character}\n");

                    foreach (var profession in character.WowCharacterProfessions)
                    {
                        message.Append($"\t{profession.WowProfession}\n");
                    }
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(message.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("list")]
            [Description("Lists your character's profressions")]
            public async Task List(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character)
            {
                StringBuilder message = new StringBuilder();

                message.Append($"{character}\n");
                foreach (var profession in character.Character.WowCharacterProfessions)
                {
                    message.Append($"\t{profession.WowProfession}\n");
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(message.ToString());

                ResponseString = "Command executed. Check your DMs.";
            }

            [Command("add")]
            [Description("Adds a profession to the specified character")]
            public async Task Add(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession)
            {
                if (character.Character.WowCharacterProfessions.Count == 2)
                {
                    ResponseString = $"{character.Character} already has two professions added. Pleae remove one first.";
                    return;
                }
                else if (character.Character.WowCharacterProfessions.Any(x => x.WowProfessionId == profession.Id))
                {
                    ResponseString = $"{character.Character} already has this profession added.";
                    return;
                }

                m_dbContext.WowCharacterProfessions.Add(new WowCharacterProfession()
                {
                    WowCharacterId = character.Character.Id,
                    WowProfessionId = profession.Id,
                });

                await m_dbContext.SaveChangesAsync();
            }

            [Command("remove")]
            [Description("Removes a profession to the specified character")]
            public async Task Remove(CommandContext context,
                [Description(Descriptions.WowCharacter)] UserWowCharacter character,
                [Description(Descriptions.WowProfession)] WowProfession profession)
            {
                var charProfession = character.Character.WowCharacterProfessions
                    .FirstOrDefault(x => x.WowProfessionId == profession.Id);

                if (charProfession != null)
                {
                    m_dbContext.WowCharacterProfessions.Remove(charProfession);
                    await m_dbContext.SaveChangesAsync();
                }
            }
        }

        [Group("item")]
        [Description("Commands for searching wow items")]
        public class WowItemCommands : BaseInciteCommand
        {
            readonly WowHeadService m_wowHead;

            public WowItemCommands(WowHeadService wowHead)
            {
                m_wowHead = wowHead;
            }

            [Command("link")]
            [Description("Gets the WowHead link for the given item")]
            public async Task Link(CommandContext context,
                WowItem item)
            {
            }
        }
    }
}
