using DSharpPlus;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class MemberHandlers : BaseHandler
    {
        readonly InciteDbContext m_dbContext;

        public MemberHandlers(DiscordClient client, InciteDbContext dbContext)
        {
            m_dbContext = dbContext;

            client.GuildCreated += Client_GuildCreated;
            client.GuildAvailable += Client_GuildAvailable;
        }

        async Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            await CreateGuildAsyc(e);
        }

        async Task Client_GuildCreated(GuildCreateEventArgs e)
        {
            await CreateGuildAsyc(e);
        }

        async Task CreateGuildAsyc(GuildCreateEventArgs e)
        {
            if (! await m_dbContext.Guilds.AnyAsync(x => x.DiscordId == e.Guild.Id))
            {
                var guild = new Guild()
                {
                    DiscordId = e.Guild.Id
                };

                var everyoneRole = new Role()
                {
                    DiscordId = e.Guild.EveryoneRole.Id,
                    Guild = guild,
                    Kind = RoleKind.Everyone
                };

                m_dbContext.Guilds.Add(guild);
                m_dbContext.Roles.Add(everyoneRole);

                await m_dbContext.SaveChangesAsync();
            }
        }

        async Task CreateGuildMembersAsync(GuildCreateEventArgs e)
        {
            var existingUsers = await m_dbContext.Users
                .Where(x => x.DiscordId == e.Guild.Id)
                .ToArrayAsync();
        }
    }
}
