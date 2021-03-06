﻿using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Converters
{
    public class WowServerConverter : IArgumentConverter<WowServer>
    {
        public async Task<Optional<WowServer>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<WowServer>();
            }

            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var server = await dbContext.WowServers
                .FirstOrDefaultAsync(x => EF.Functions.ILike(x.Name, value));

            if (server == null)
            {
                await ctx.Message.RespondAsync($"Server not found");
                return Optional.FromNoValue<WowServer>();
            }

            return Optional.FromValue(server);
        }
    }
}
