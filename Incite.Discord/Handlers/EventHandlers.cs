using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Incite.Discord.Messages;
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
            using (var messageLock = await LockOnMessageAsync(e.Message.Id))
            {
                var message = await EventMessage.TryCreateFromMessageAsync(e.Client, e.Message);
                if (message != null && e.User != e.Client.CurrentUser)
                {
                    await message.RemovePreviousReactionsAsync(e.User, e.Emoji);
                    await message.AddUserAsync(e.User, e.Emoji);
                }
            }
        }

        private async Task Client_MessageReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            using (var messageLock = await LockOnMessageAsync(e.Message.Id))
            {
                var message = await EventMessage.TryCreateFromMessageAsync(e.Client, e.Message);
                if (message != null)
                {
                    await message.RemoveUserAsync(e.User, e.Emoji);
                }
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
