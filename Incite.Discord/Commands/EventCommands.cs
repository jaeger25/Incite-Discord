using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.ApiModels;
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
    [Description("Commands for setting and retrieving info about the guild's events")]
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
            [Description(Descriptions.DateTime)] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                ResponseString = "The event cannot be in the past";
                return;
            }

            var discordMessage = await context.Message.RespondAsync("\u200b");

            var channel = await m_dbContext.Channels
                .FirstOrDefaultAsync(x => x.DiscordId == context.Channel.Id);

            var message = new Message()
            {
                DiscordId = discordMessage.Id,
            };

            if (channel == null)
            {
                message.Channel = new Channel()
                {
                    DiscordId = context.Channel.Id,
                    GuildId = Guild.Id
                };

                m_dbContext.Channels.Add(message.Channel);
            }
            else
            {
                message.ChannelId = channel.Id;
            }

            var memberEvent = new MemberEvent()
            {
                MemberId = Member.Id,
                Event = new Models.Event()
                {
                    Name = name,
                    Description = description,
                    DateTime = dateTime,
                    GuildId = Guild.Id
                }
            };

            memberEvent.Event.EventMessage = new Models.EventMessage()
            {
                Event = memberEvent.Event,
                Message = message
            };

            Member.MemberEvents.Add(memberEvent);
            await m_dbContext.SaveChangesAsync();

            var eventMessage = new Messages.EventMessage(context.Client, discordMessage, memberEvent.Event);
            await eventMessage.UpdateAsync();

            await eventMessage.AddReactionsToEventMessageAsync();
            ResponseString = "";
        }

        [Command("update")]
        [RequireUserPermissions(Permissions.SendMessages)]
        [Description("Updates an event")]
        public async Task Update(CommandContext context,
            int eventId,
            string name,
            string description,
            [Description(Descriptions.DateTime)] DateTimeOffset dateTime)
        {
            if (DateTimeOffset.UtcNow > dateTime.UtcDateTime)
            {
                ResponseString = "The event cannot be in the past";
                return;
            }

            await context.Message.DeleteAsync();

            var memberEvent = Guild.Events
                .FirstOrDefault(x => x.Id == eventId);

            if (memberEvent == null)
            {
                ResponseString = "Failed to find event";
                return;
            }

            var discordMessage = await context.Channel.GetMessageAsync(memberEvent.EventMessage.Message.DiscordId);

            memberEvent.Name = name;
            memberEvent.Description = description;
            memberEvent.DateTime = dateTime;

            await m_dbContext.SaveChangesAsync();

            var eventMessage = new Messages.EventMessage(context.Client, discordMessage, memberEvent);
            await eventMessage.UpdateAsync();
            ResponseString = "";
        }
    }
}
