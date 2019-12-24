using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        public GuildHandlers(DiscordClient client, GuildCommandPrefixCache commandPrefixCache)
        {
            m_commandPrefixCache = commandPrefixCache;

            client.GuildCreated += Client_GuildCreated;
            client.GuildAvailable += Client_GuildAvailable;
            client.GuildMemberAdded += Client_GuildMemberAdded;
        }

        Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            _ = Task.Run(async () =>
            {
                var dbContext = e.Client.GetCommandsNext().Services.GetService<InciteDbContext>();
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

        Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            _ = UpdateGuildEntitiesAsync(e);
            return Task.CompletedTask;
        }

        Task Client_GuildCreated(GuildCreateEventArgs e)
        {
            _ = UpdateGuildEntitiesAsync(e);
            return Task.CompletedTask;
        }

        async Task UpdateGuildEntitiesAsync(GuildCreateEventArgs e)
        {
            var dbContext = e.Client.GetCommandsNext().Services.GetService<InciteDbContext>();
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
        }

        async Task CreateNewGuildMembersAsync(Guild guild, GuildCreateEventArgs e, InciteDbContext dbContext)
        {
            var memberDiscordIds = e.Guild.Members
                .Select(x => x.Key);

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
