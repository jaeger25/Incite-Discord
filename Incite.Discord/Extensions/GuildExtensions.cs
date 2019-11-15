using DSharpPlus.CommandsNext;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Extensions
{
    public static class GuildExtensions
    {
        public static Task<Guild> GetCurrentGuildAsync(this DbSet<Guild> guilds, UInt64 discordGuildId)
        {
            return guilds
                .FirstAsync(x => x.DiscordId == discordGuildId);
        }

        public static Task<Guild> GetCurrentGuildAsync(this DbSet<Guild> guilds, CommandContext context)
        {
            return guilds.GetCurrentGuildAsync(context.Guild.Id);
        }
    }
}
