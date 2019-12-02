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
                .Where(x => x.Name.Equals(nameServer[0], StringComparison.OrdinalIgnoreCase))
                .ToArrayAsync();

            if (characters.Length != 1 && nameServer.Length == 2)
            {
                characters = characters
                    .Where(x => x.WowServer.Name.Equals(nameServer[1], StringComparison.OrdinalIgnoreCase))
                    .ToArray();
            }

            if (characters.Length != 1)
            {
                return Optional.FromNoValue<WowCharacter>();
            }

            return Optional.FromValue(characters[0]);
        }
    }
}
