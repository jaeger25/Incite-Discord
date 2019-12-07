using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Converters
{
    public class WowCharacterConverter : IArgumentConverter<WowCharacter>
    {
        public async Task<Optional<WowCharacter>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<WowCharacter>();
            }

            var dbContext = ctx.Services.GetService<InciteDbContext>();

            var nameServer = value.Split('-');
            var characters = await dbContext.WowCharacters
                .Where(x => EF.Functions.ILike(x.Name, nameServer[0]))
                .ToArrayAsync();

            if (characters.Length != 1 && nameServer.Length == 2)
            {
                characters = characters
                    .Where(x => x.WowServer.Name.ToLower() == nameServer[1].ToLower())
                    .ToArray();
            }

            if (characters.Length != 1)
            {
                await ctx.Message.RespondAsync($"No character named {value} found for {ctx.Member.DisplayName}. Must be in the form CharName-ServerName.");
                return Optional.FromNoValue<WowCharacter>();
            }

            return Optional.FromValue(characters[0]);
        }
    }
}
