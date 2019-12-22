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
    [RequireWowCharacter]
    [ModuleLifespan(ModuleLifespan.Transient)]
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

            var message = new Message()
            {
                DiscordId = discordMessage.Id,
            };

            var memberEvent = new Models.Event()
            {
                Name = name,
                Description = description,
                DateTime = dateTime,
                GuildId = Guild.Id,
                OwnerId = Member.Id,
                EventMessage = message
            };

            m_dbContext.Events.Add(memberEvent);
            await m_dbContext.SaveChangesAsync();

            var discordEventMessage = new Messages.DiscordEventMessage(context.Client, discordMessage, memberEvent);
            await discordEventMessage.UpdateAsync();

            await discordEventMessage.AddReactionsToEventMessageAsync();
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

            var memberEvent = Member.OwnedEvents
                .FirstOrDefault(x => x.Id == eventId);

            if (memberEvent == null)
            {
                ResponseString = "Failed to find event";
                return;
            }

            var discordMessage = await context.Channel.GetMessageAsync(memberEvent.EventMessage.DiscordId);

            memberEvent.Name = name;
            memberEvent.Description = description;
            memberEvent.DateTime = dateTime;

            await m_dbContext.SaveChangesAsync();

            var eventMessage = new DiscordEventMessage(context.Client, discordMessage, memberEvent);
            await eventMessage.UpdateAsync();
        }
    }
}
