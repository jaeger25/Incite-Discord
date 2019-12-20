using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Models;
using Incite.Discord.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Incite.Discord.Attributes
{
    public class RequireWowCharacterAttribute : CheckBaseAttribute
    {
        public override async Task<bool> ExecuteCheckAsync(CommandContext context, bool help)
        {
            var dbContext = context.Services.GetService<InciteDbContext>();

            bool hasCharacter = await dbContext.WowCharacters
                .AnyAsync(x => x.User.DiscordId == context.User.Id);

            if (!hasCharacter)
            {
                await context.Message.RespondAsync("You must add a character using '!wow character add charName-ServerName Class Faction' first");
            }

            return hasCharacter;
        }
    }
}
