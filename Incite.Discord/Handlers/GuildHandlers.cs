using DSharpPlus;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Services;
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
        readonly GuildCommandPrefixCache m_commandPrefixCache;

        public GuildHandlers(DiscordClient client, InciteDbContext dbContext, GuildCommandPrefixCache commandPrefixCache)
        {
            m_dbContext = dbContext;
            m_commandPrefixCache = commandPrefixCache;

            client.GuildCreated += Client_GuildCreated;
            client.GuildAvailable += Client_GuildAvailable;
            client.GuildMemberAdded += Client_GuildMemberAdded;
        }

        async Task Client_GuildMemberAdded(GuildMemberAddEventArgs e)
        {
            var user = m_dbContext.Users
                .Include(x => x.Memberships)
                    .ThenInclude(x => x.Guild)
                .FirstOrDefault(x => x.DiscordId == e.Member.Id);

            Member member = user?.Memberships
                .FirstOrDefault(x => x.Guild.DiscordId == e.Guild.Id);

            if (member == null)
            {
                member = new Member()
                {
                    Guild = await m_dbContext.Guilds.FirstAsync(x => x.DiscordId == e.Guild.Id),
                    User = user ?? new User()
                    {
                        DiscordId = e.Member.Id
                    }
                };

                m_dbContext.Members.Add(member);
                await m_dbContext.SaveChangesAsync();
            }
        }

        async Task Client_GuildAvailable(GuildCreateEventArgs e)
        {
            await UpdateGuildEntitiesAsync(e);
        }

        async Task Client_GuildCreated(GuildCreateEventArgs e)
        {
            await UpdateGuildEntitiesAsync(e);
        }

        async Task UpdateGuildEntitiesAsync(GuildCreateEventArgs e)
        {
            var guild = await m_dbContext.Guilds
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

                m_dbContext.Guilds.Add(guild);
                m_dbContext.Roles.Add(everyoneRole);
            }

            if (guild.Options == null)
            {
                guild.Options = new GuildOptions()
                {
                    CommandPrefix = '!'
                };
            }

            m_commandPrefixCache.SetPrefix(guild, guild.Options.CommandPrefix);
            await CreateNewGuildMembersAsync(guild, e);

            await m_dbContext.SaveChangesAsync();
        }

        async Task CreateNewGuildMembersAsync(Guild guild, GuildCreateEventArgs e)
        {
            var memberDiscordIds = e.Guild.Members
                .Select(x => x.Key);

            var existingUsers = await m_dbContext.Users
                .Where(x => memberDiscordIds
                    .Contains(x.DiscordId))
                .ToArrayAsync();

            var existingMembers = await m_dbContext.Members
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

            m_dbContext.Users.AddRange(newUsers);

            var newMembers = memberDiscordIds
                .Where(x => !existingMembers
                    .Select(x => x.User.DiscordId)
                    .Contains(x))
                .Select(x => new Member()
                {
                    Guild = guild,
                    User = m_dbContext.Users.Local.First(y => y.DiscordId == x)
                });

            m_dbContext.Members.AddRange(newMembers);
        }
    }
}
