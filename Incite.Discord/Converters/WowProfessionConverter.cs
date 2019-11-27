﻿using DSharpPlus.CommandsNext;
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
    public class WowProfessionConverter : IArgumentConverter<WowProfession>
    {
        public async Task<Optional<WowProfession>> ConvertAsync(string value, CommandContext ctx)
        {
            var dbContext = ctx.Services.GetService<InciteDbContext>();
            var profession = await dbContext.WowProfessions
                .FirstOrDefaultAsync(x => x.Name == value);

            if (profession == null)
            {
                return Optional.FromNoValue<WowProfession>();
            }

            return Optional.FromValue(profession);
        }
    }
}