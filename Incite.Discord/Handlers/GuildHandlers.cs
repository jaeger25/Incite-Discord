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
    public class GuildHandlers : BaseHandler
    {
        readonly InciteDbContext m_dbContext;

        public GuildHandlers(DiscordClient client, InciteDbContext dbContext)
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
                m_dbContext.Guilds.Add(new Guild()
                {
                    DiscordId = e.Guild.Id
                });

                await m_dbContext.SaveChangesAsync();
            }
        }
    }
}
