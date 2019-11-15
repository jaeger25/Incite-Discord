using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("admin")]
    [RequireOwner]
    [RequireGuild]
    [Description("Commands for managing bot settings for this guild")]
    public class AdminCommands : BaseCommandModule
    {
        [Command("setrolename")]
        [Description("Sets the server role which corresponds with the RoleKind")]
        public async Task SetRoleName(CommandContext context,
            [Description("Values: Member, Officer")] RoleKind roleKind,
            [Description("Server role")] DiscordRole role)
        {
            using var dbContext = new InciteDbContext();
            var existingRole = await dbContext.Roles
                .FirstOrDefaultAsync(x => x.Guild.DiscordId == context.Guild.Id && x.Kind == roleKind);

            if (existingRole == null)
            {
                dbContext.Roles.Add(new Role()
                {
                    DiscordId = role.Id,
                    GuildId = (await dbContext.Guilds.GetCurrentGuildAsync(context)).Id,
                    Kind = roleKind
                });
            }
            else
            {
                existingRole.DiscordId = role.Id;
                dbContext.Roles.Update(existingRole);
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
