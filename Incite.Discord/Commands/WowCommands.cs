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
                    characterList.Append($"\n{character} , {character.WowClass}\n");
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
                User.WowCharacters.Add(new WowCharacter()
                {
                    Name = character.Name,
                    WowFaction = faction,
                    GuildId = Guild.Id,
                    UserId = User.Id,
                    WowClassId = characterClass.Id,
                    WowServerId = character.Server.Id,
                });

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
    }
}
