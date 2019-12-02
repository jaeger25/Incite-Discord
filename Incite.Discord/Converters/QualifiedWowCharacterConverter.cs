using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.ApiModels;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Converters
{
    public class QualifiedWowCharacterConverter : IArgumentConverter<QualifiedWowCharacter>
    {
        public async Task<Optional<QualifiedWowCharacter>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<QualifiedWowCharacter>();
            }

            var nameServerPair = value.Split('-');
            if (nameServerPair.Length != 2)
            {
                return Optional.FromNoValue<QualifiedWowCharacter>();
            }

            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var server = await dbContext.WowServers
                .FirstOrDefaultAsync(x => x.Name == nameServerPair[1]);

            if (server == null)
            {
                return Optional.FromNoValue<QualifiedWowCharacter>();
            }

            return Optional.FromValue(new QualifiedWowCharacter()
            {
                Name = nameServerPair[0],
                Server = server
            });
        }
    }
}
