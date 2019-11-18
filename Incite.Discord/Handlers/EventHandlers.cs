using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Nito.AsyncEx;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class EventHandlers : BaseHandler
    {
        readonly InciteDbContext m_dbContext;

        Dictionary<ulong, AsyncLock> m_eventMessageLocks = new Dictionary<ulong, AsyncLock>();

        public EventHandlers(DiscordClient client, InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
            client.MessageReactionAdded += Client_MessageReactionAdded;
            client.MessageReactionRemoved += Client_MessageReactionRemoved;
        }

        private async Task Client_MessageReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.User.IsCurrent || !e.Message.Author.IsCurrent)
            {
                return;
            }

            var guildEvent = await m_dbContext.Events
                .FirstOrDefaultAsync(x => x.Message.DiscordId == e.Message.Id);

            if (guildEvent == null)
            {
                return;
            }

            var member = await m_dbContext.Members.TryGetMemberAsync(e.Guild.Id, e.User.Id);
            if (e.User != e.Client.CurrentUser && member == null)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User, "User not registered");

                var discordMember = await e.Guild.GetMemberAsync(e.User.Id);
                var dmChannel = await discordMember.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("You must first register with the '!member register' command");
                return;
            }

            using var messageLock = await LockOnMessageAsync(e.Message.Id);

            var eventMember = guildEvent.EventMembers
                .FirstOrDefault(x => x.MemberId == member.Id);

            if (eventMember == null)
            {
                eventMember = new EventMember()
                {
                    EventId = guildEvent.Id,
                    MemberId = member.Id,
                    EmojiDiscordId = e.Emoji.Id
                };
            }

            var message = await e.Message.HydrateAsync();
            var eventMessage = new EventMessage(e.Client, e.Message);
            await eventMessage.RemovePreviousReactionsAsync(e.User, e.Emoji);

            await eventMessage.AddUserAsync(member, e.Emoji);
        }

        private async Task Client_MessageReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            if (!e.Message.Author.IsCurrent)
            {
                return;
            }

            var guildEvent = m_dbContext.Events
                .FirstOrDefaultAsync(x => x.Message.DiscordId == e.Message.Id);

            if (guildEvent == null)
            {
                return;
            }

            var eventMessage = new EventMessage(e.Client, e.Message);
            var member = await m_dbContext.Members.TryGetMemberAsync(e.Guild.Id, e.User.Id);
            if (eventMessage != null && member != null)
            {
                await eventMessage.RemoveUserAsync(member, e.Emoji);
            }
        }

        private async Task<IDisposable> LockOnMessageAsync(ulong messageId)
        {
            AsyncLock messageLock;
            lock (m_eventMessageLocks)
            {
                if (!m_eventMessageLocks.TryGetValue(messageId, out messageLock))
                {
                    messageLock = new AsyncLock();
                    m_eventMessageLocks.Add(messageId, messageLock);
                }
            }

            return await messageLock.LockAsync();
        }
    }
}
