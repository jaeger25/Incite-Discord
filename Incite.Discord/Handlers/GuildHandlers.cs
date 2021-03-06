﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class GuildHandlers : BaseHandler
    {
        readonly GuildCommandPrefixCache m_commandPrefixCache;
        readonly ILogger<DiscordService> m_logger;

        public GuildHandlers(ILogger<DiscordService> logger, IServiceScopeFactory scopeFactory, DiscordClient client, GuildCommandPrefixCache commandPrefixCache) : base(scopeFactory)
        {
            m_commandPrefixCache = commandPrefixCache;
            m_logger = logger;

            client.GuildCreated += Client_GuildCreated;
            client.GuildAvailable += Client_GuildAvailable;
            client.GuildMemberAdded += Client_GuildMemberAdded;
        }

        Task Client_GuildMemberAdded(DiscordClient client, GuildMemberAddEventArgs e)
        {
            _ = Task.Run(async () =>
            {
                using var scope = ServiceScopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetService<InciteDbContext>();
                var user = dbContext.Users
                    .Include(x => x.Memberships)
                        .ThenInclude(x => x.Guild)
                    .FirstOrDefault(x => x.DiscordId == e.Member.Id);

                Member member = user?.Memberships
                    .FirstOrDefault(x => x.Guild.DiscordId == e.Guild.Id);

                if (member == null)
                {
                    member = new Member()
                    {
                        Guild = await dbContext.Guilds.FirstAsync(x => x.DiscordId == e.Guild.Id),
                        User = user ?? new User()
                        {
                            DiscordId = e.Member.Id
                        }
                    };

                    dbContext.Members.Add(member);
                    await dbContext.SaveChangesAsync();
                }
            });

            return Task.CompletedTask;
        }

        Task Client_GuildAvailable(DiscordClient client, GuildCreateEventArgs e)
        {
            _ = UpdateGuildEntitiesAsync(e);
            return Task.CompletedTask;
        }

        Task Client_GuildCreated(DiscordClient client, GuildCreateEventArgs e)
        {
            _ = UpdateGuildEntitiesAsync(e);
            return Task.CompletedTask;
        }

        async Task UpdateGuildEntitiesAsync(GuildCreateEventArgs e)
        {
            m_logger.LogCritical($"UpdateGuildEntitiesAsync-Start: {e.Guild.Name}");
            using var scope = ServiceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<InciteDbContext>();
            var guild = await dbContext.Guilds
                .FirstOrDefaultAsync(x => x.DiscordId == e.Guild.Id);

            if (guild == null)
            {
                guild = new Guild()
                {
                    DiscordId = e.Guild.Id
                };

                var everyoneRole = new Role()
                {
                    DiscordId = e.Guild.EveryoneRole.Id,
                    Guild = guild,
                    Kind = RoleKind.Everyone
                };

                dbContext.Guilds.Add(guild);
                dbContext.Roles.Add(everyoneRole);
            }

            if (guild.Options == null)
            {
                guild.Options = new GuildOptions()
                {
                    CommandPrefix = '!'
                };
            }

            m_commandPrefixCache.SetPrefix(guild, guild.Options.CommandPrefix);
            await CreateNewGuildMembersAsync(guild, e, dbContext);

            await dbContext.SaveChangesAsync();
            m_logger.LogCritical($"UpdateGuildEntitiesAsync-Stop: {e.Guild.Name}");
        }

        async Task CreateNewGuildMembersAsync(Guild guild, GuildCreateEventArgs e, InciteDbContext dbContext)
        {
            var members = await e.Guild.GetAllMembersAsync();

            var memberDiscordIds = members
                .AsEnumerable()
                .Select(x => x.Id);

            var existingUsers = await dbContext.Users
                .Where(x => memberDiscordIds
                    .Contains(x.DiscordId))
                .ToArrayAsync();

            var existingMembers = await dbContext.Members
                .Include(x => x.User)
                .Where(x => x.Guild.DiscordId == guild.DiscordId && memberDiscordIds
                    .Contains(x.User.DiscordId))
                .ToArrayAsync();

            var newUsers = memberDiscordIds
                .Where(x => !existingUsers
                    .Select(x => x.DiscordId)
                    .Contains(x))
                .Select(x => new User()
                {
                    DiscordId = x
                });

            dbContext.Users.AddRange(newUsers);

            var newMembers = memberDiscordIds
                .Where(x => !existingMembers
                    .Select(x => x.User.DiscordId)
                    .Contains(x))
                .Select(x => new Member()
                {
                    Guild = guild,
                    User = dbContext.Users.Local.First(y => y.DiscordId == x)
                });

            dbContext.Members.AddRange(newMembers);
        }
    }
}
