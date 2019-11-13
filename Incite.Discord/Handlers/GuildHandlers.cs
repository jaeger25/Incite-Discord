﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class GuildHandlers : BaseHandler
    {
        public GuildHandlers(DiscordClient client)
        {
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
            using var dbContext = e.Client.GetExtension<DatabaseExtension>().GetDbContext();

            var guild = dbContext.Guilds
                .FirstOrDefault(x => x.DiscordGuildId == e.Guild.Id);

            if (guild == null)
            {
                dbContext.Guilds.Add(new Guild()
                {
                    DiscordGuildId = e.Guild.Id
                });

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
