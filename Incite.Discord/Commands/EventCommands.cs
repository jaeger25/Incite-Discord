using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("event")]
    [RequireGuildConfigured]
    [RequireMemberRegistered]
    [Description("Commands for setting and retrieving info about the guild's schedule")]
    public class EventCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public EventCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("create")]
        [RequireUserPermissions(Permissions.SendMessages)]
        [Description("Creates an event")]
        public async Task Create(CommandContext context,
            string name,
            string description,
            [Description("Format: \"10-31-2019 9:00 PM -04:00\"")] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                var dmChannel = await context.Member.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("The event cannot be in the past.");
                return;
            }

            var discordMessage = await context.Message.RespondAsync("\u200b");
            await context.Message.DeleteAsync();

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            var channel = await m_dbContext.Channels
                .FirstOrDefaultAsync(x => x.DiscordId == context.Channel.Id);

            var creator = guild.Members
                .First(x => x.User.DiscordId == context.User.Id);

            var message = new Message()
            {
                DiscordId = discordMessage.Id,
            };

            if (channel == null)
            {
                message.Channel = new Channel()
                {
                    DiscordId = context.Channel.Id,
                    GuildId = guild.Id
                };

                m_dbContext.Channels.Add(message.Channel);
            }
            else
            {
                message.ChannelId = channel.Id;
            }

            var memberEvent = new MemberEvent()
            {
                MemberId = creator.Id,
                Event = new Models.Event()
                {
                    Name = name,
                    Description = description,
                    DateTime = dateTime,
                    GuildId = guild.Id
                }
            };

            memberEvent.Event.EventMessage = new Models.EventMessage()
            {
                Event = memberEvent.Event,
                Message = message
            };

            creator.MemberEvents.Add(memberEvent);
            await m_dbContext.SaveChangesAsync();

            var eventMessage = new Messages.EventMessage(context.Client, discordMessage, memberEvent.Event);
            await eventMessage.UpdateAsync();

            await eventMessage.AddReactionsToEventMessageAsync();
        }

        [Command("update")]
        [RequireUserPermissions(Permissions.SendMessages)]
        [Description("Updates an event")]
        public async Task Update(CommandContext context,
            int eventId,
            string name,
            string description,
            [Description("Format: \"10-31-2019 9:00 PM -05:00\"")] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                var dmChannel = await context.Member.CreateDmChannelAsync();

                await dmChannel.SendMessageAsync("The event cannot be in the past.");
                return;
            }

            await context.Message.DeleteAsync();

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            var memberEvent = guild.Events
                .FirstOrDefault(x => x.Id == eventId);

            if (memberEvent == null)
            {
                await context.Message.RespondAsync("Failed to find event");
                return;
            }

            var discordMessage = await context.Channel.GetMessageAsync(memberEvent.EventMessage.Message.DiscordId);

            memberEvent.Name = name;
            memberEvent.Description = description;
            memberEvent.DateTime = dateTime;

            await m_dbContext.SaveChangesAsync();

            var eventMessage = new Messages.EventMessage(context.Client, discordMessage, memberEvent);
            await eventMessage.UpdateAsync();
        }
    }
}
