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
    [Group("wow")]
    [Description("Commands for managing info related to in-game WoW concepts")]
    public class WowCommands : BaseCommandModule
    {
        readonly InciteDbContext m_dbContext;

        public WowCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("add-character")]
        [Description("Adds a character to your character list")]
        public async Task AddCharacter(CommandContext context,
            string characterName,
            string characterClass,
            string serverName)
        {
            var server = m_dbContext.WowServers
                .FirstOrDefaultAsync(x => x.Name == serverName);

            if (server == null)
            {
                await context.Message.RespondAsync($"Unable to finder server: {serverName}");
                return;
            }

            var user = await m_dbContext.Users.TryGetCurrentUserAsync(context);
            if (user == null)
            {
                // TODO
                return;
            }

            user.WowCharacters.Add(new WowCharacter()
            {
                Name = characterName,
                UserId = user.Id,
                WowClassId = 0, // TODO: Convert to enum
                WowServerId = server.Id,
            });

            await m_dbContext.SaveChangesAsync();
        }

        [Command("remove-character")]
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
