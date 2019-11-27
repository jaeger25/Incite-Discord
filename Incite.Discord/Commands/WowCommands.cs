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
                StringBuilder characterList = new StringBuilder();
                HashSet<string> servers = new HashSet<string>();
                foreach (var character in User.WowCharacters.OrderBy(x => x.WowServerId))
                {
                    if (!servers.Contains(character.WowServer.Name))
                    {
                        characterList.Append($"\n__Id__ - __Name__ - __Server__\n");
                        servers.Add(character.WowServer.Name);
                    }
                    characterList.Append($"{character.Name} - {character.WowClass.Name} - {character.WowServer.Name}\n");
                }

                var dmChannel = await context.Member.CreateDmChannelAsync();
                await dmChannel.SendMessageAsync(characterList.ToString());
            }

            [Command("add")]
            [Description("Adds a character to your character list")]
            public async Task AddCharacter(CommandContext context,
                QualifiedWowCharacter character,
                WowClass characterClass)
            {
                var guild = await m_dbContext.Guilds.GetCurrentGuildAsync(context);

                User.WowCharacters.Add(new WowCharacter()
                {
                    Name = character.Name,
                    GuildId = guild.Id,
                    UserId = User.Id,
                    WowClassId = characterClass.Id,
                    WowServerId = character.Server.Id,
                });

                await m_dbContext.SaveChangesAsync();
            }

            [Command("remove")]
            [Description("Removes a character from your character list")]
            public async Task RemoveCharacter(CommandContext context, UserWowCharacter character)
            {
                m_dbContext.WowCharacters.Remove(character.Character);
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-class")]
            [Description("Sets a property for one of your characters")]
            public async Task SetClass(CommandContext context,
                UserWowCharacter character,
                WowClass wowClass)
            {
                character.Character.WowClassId = wowClass.Id;
                await m_dbContext.SaveChangesAsync();
            }

            [Command("set-server")]
            [Description("Sets a property for one of your characters")]
            public async Task SetServer(CommandContext context,
                UserWowCharacter character,
                WowServer server)
            {
                character.Character.WowServerId = server.Id;
                await m_dbContext.SaveChangesAsync();
            }
        }
    }
}
