using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.Helpers;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("member")]
    [RequireGuild]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for querying individual member information")]
    public class MemberCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public MemberCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list-characters")]
        [Description("Lists the member's characters")]
        public async Task List(CommandContext context, DiscordUser discordUser)
        {
            var user = await m_dbContext.Users
                .Include(x => x.WowCharacters)
                    .ThenInclude(x => x.WowClass)
                .FirstOrDefaultAsync(x => x.DiscordId == discordUser.Id);

            if (user == null)
            {
                ResponseString = "User not found";
                return;
            }

            StringBuilder characterList = new StringBuilder("__**Character**__ , __**Class**__");
            foreach (var character in User.WowCharacters.OrderBy(x => x.WowServerId))
            {
                characterList.Append($"\n{character} , {character.WowClass}");
            }

            await context.RespondAsync(characterList.ToString());
        }
    }
}
