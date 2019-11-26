using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("wow-character")]
    [Description("Commands for managing info related to in-game WoW concepts")]
    public class WowCharacterCommands : BaseCommandModule
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
            var user = await m_dbContext.Users.TryGetCurrentUserAsync(context);
            if (user == null)
            {
                return;
            }

            StringBuilder characterList = new StringBuilder();
            HashSet<string> servers = new HashSet<string>();
            foreach(var character in user.WowCharacters.OrderBy(x => x.WowServerId))
            {
                if (!servers.Contains(character.WowServer.Name))
                {
                    characterList.Append($"\n__{character.WowServer.Name}__\n");
                    servers.Add(character.WowServer.Name);
                }
                characterList.Append($"{character.Name} - {character.WowClass.Name}\n");
            }

            var dmChannel = await context.Member.CreateDmChannelAsync();
            await dmChannel.SendMessageAsync(characterList.ToString());
        }

        [Command("add")]
        [Description("Adds a character to your character list")]
        public async Task AddCharacter(CommandContext context,
            string characterName,
            string characterClass,
            string serverName)
        {
            var server = await m_dbContext.WowServers
                .FirstOrDefaultAsync(x => x.Name == serverName);

            var wowClass = await m_dbContext.WowClasses
                .FirstOrDefaultAsync(x => x.Name == characterClass);

            if (wowClass == null)
            {
                await context.Message.RespondAsync($"Unable to finder class: {characterClass}");
                return;
            }

            if (server == null)
            {
                await context.Message.RespondAsync($"Unable to finder server: {serverName}");
                return;
            }

            var user = await m_dbContext.Users.TryGetCurrentUserAsync(context);
            if (user == null)
            {
                return;
            }

            user.WowCharacters.Add(new WowCharacter()
            {
                Name = characterName,
                UserId = user.Id,
                WowClassId = wowClass.Id,
                WowServerId = server.Id,
            });

            await m_dbContext.SaveChangesAsync();
        }

        [Command("remove")]
        [Description("Removes a character from your character list")]
        public async Task RemoveCharacter(CommandContext context,
            string characterName,
            string serverName)
        {
            var user = await m_dbContext.Users.TryGetCurrentUserAsync(context);
            if (user == null)
            {
                // TODO
                return;
            }

            var character = user.WowCharacters
                .FirstOrDefault(x => x.Name == characterName && x.WowServer.Name == serverName);

            if (character == null)
            {
                await context.RespondAsync($"Unable to find character {characterName} on {serverName}");
                return;
            }

            m_dbContext.WowCharacters.Remove(character);
            await m_dbContext.SaveChangesAsync();
        }
    }
}
