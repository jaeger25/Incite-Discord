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

            WowServer server = null;

            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var nameServerPair = value.Split('-');
            if (nameServerPair.Length != 2)
            {
                var guild = await dbContext.Guilds
                    .Include(x => x.WowServer)
                    .FirstOrDefaultAsync(x => x.DiscordId == ctx.Guild.Id);

                if (guild?.WowServerId.HasValue == false)
                {
                    await ctx.Message.RespondAsync("QualifiedWowCharacter must be in the form CharName-ServerName");
                    return Optional.FromNoValue<QualifiedWowCharacter>();
                }

                server = guild.WowServer;
            }

            server = await dbContext.WowServers
                .FirstOrDefaultAsync(x => EF.Functions.ILike(x.Name, nameServerPair[1]));

            if (server == null)
            {
                await ctx.Message.RespondAsync("Server not found");
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
