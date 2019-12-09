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
    public class WowClassConverter : IArgumentConverter<WowClass>
    {
        public async Task<Optional<WowClass>> ConvertAsync(string value, CommandContext ctx)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Optional.FromNoValue<WowClass>();
            }

            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var wowClass = await dbContext.WowClasses
                .FirstOrDefaultAsync(x => EF.Functions.ILike(x.Name, value));

            if (wowClass == null)
            {
                await ctx.Message.RespondAsync($"WowClass not found.");
                return Optional.FromNoValue<WowClass>();
            }

            return Optional.FromValue(wowClass);
        }
    }
}
