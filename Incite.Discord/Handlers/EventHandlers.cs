using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Models;
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
        Dictionary<ulong, AsyncLock> m_eventMessageLocks = new Dictionary<ulong, AsyncLock>();

        public EventHandlers(DiscordClient client)
        {
            client.MessageReactionAdded += Client_MessageReactionAdded;
            client.MessageReactionRemoved += Client_MessageReactionRemoved;
        }

        private async Task Client_MessageReactionAdded(MessageReactionAddEventArgs e)
        {
            if (e.User.IsCurrent)
            {
                return;
            }

            using var dbContext = new InciteDbContext();

            var member = await dbContext.Members.TryGetMemberAsync(e.Guild.Id, e.User.Id);
            if (e.User != e.Client.CurrentUser && member == null)
            {
                await e.Message.DeleteReactionAsync(e.Emoji, e.User, "User not registered");

                var discordMember = await e.Guild.GetMemberAsync(e.User.Id);
                var dmChannel = await discordMember.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("You must first register with the '!member register' command");
                return;
            }

            using var messageLock = await LockOnMessageAsync(e.Message.Id);
            var message = await EventMessage.TryCreateFromMessageAsync(e.Client, e.Message);
            if (message != null)
            {
                await message.RemovePreviousReactionsAsync(e.User, e.Emoji);
                await message.AddUserAsync(member, e.Emoji);
            }
        }

        private async Task Client_MessageReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            using var dbContext = new InciteDbContext();

            var message = await EventMessage.TryCreateFromMessageAsync(e.Client, e.Message);
            var member = await dbContext.Members.TryGetMemberAsync(e.Guild.Id, e.User.Id);
            if (message != null && member != null)
            {
                await message.RemoveUserAsync(member, e.Emoji);
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
