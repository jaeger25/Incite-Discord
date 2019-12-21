using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
            var message = await e.Message.HydrateAsync();
            if (e.User.IsCurrent || !message.Author.IsCurrent)
            {
                return;
            }

            var dbContext = e.Client.GetCommandsNext().Services.GetService<InciteDbContext>();
            var guildEvent = await dbContext.Events
                .Include(x => x.EventMembers)
                    .ThenInclude(x => x.Member)
                        .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.EventMessage.DiscordId == message.Id);

            if (guildEvent == null)
            {
                return;
            }

            var member = await dbContext.Members.TryGetMemberAsync(e.Guild.Id, e.User.Id);
            if (member == null || !member.PrimaryWowCharacterId.HasValue)
            {
                await message.DeleteReactionAsync(e.Emoji, e.User, "User not registered");

                var discordMember = await e.Guild.GetMemberAsync(e.User.Id);
                var dmChannel = await discordMember.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("You must first add a wow character. See '!help wow char add-character' for details");
                return;
            }

            using var messageLock = await LockOnMessageAsync(message.Id);

            var eventMember = guildEvent.EventMembers
                .FirstOrDefault(x => x.MemberId == member.Id);

            if (eventMember == null)
            {
                eventMember = new EventMember()
                {
                    EventId = guildEvent.Id,
                    MemberId = member.Id,
                    EmojiDiscordName = e.Emoji.Name
                };

                guildEvent.EventMembers.Add(eventMember);
            }
            else
            {
                eventMember.EmojiDiscordName = e.Emoji.Name;
            }

            await dbContext.SaveChangesAsync();

            var eventMessage = new Messages.DiscordEventMessage(e.Client, message, guildEvent);
            await eventMessage.RemovePreviousReactionsAsync(e.User, e.Emoji);

            await eventMessage.UpdateAsync();
        }

        private async Task Client_MessageReactionRemoved(MessageReactionRemoveEventArgs e)
        {
            var message = await e.Message.HydrateAsync();
            if (!message.Author.IsCurrent)
            {
                return;
            }

            var dbContext = e.Client.GetCommandsNext().Services.GetService<InciteDbContext>();
            var guildEvent = await dbContext.Events
                .Include(x => x.EventMembers)
                    .ThenInclude(x => x.Member)
                        .ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.EventMessage.DiscordId == message.Id);

            if (guildEvent == null)
            {
                return;
            }

            var member = guildEvent.EventMembers
                .First(x => x.Member.User.DiscordId == e.User.Id);

            if (member.EmojiDiscordName == e.Emoji.Name)
            {
                dbContext.EventMembers.Remove(member);
                await dbContext.SaveChangesAsync();
            }

            var eventMessage = new Messages.DiscordEventMessage(e.Client, message, guildEvent);
            await eventMessage.UpdateAsync();
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
