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
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("event")]
    [RequireGuildConfigured]
    [Description("Commands for setting and retrieving info about the guild's schedule")]
    public class EventCommands : BaseCommandModule
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
            var message = await context.Message.RespondAsync("EVENT");
            await context.Message.DeleteAsync();

            var channel = m_dbContext.Channels
                .FirstAsync(x => x.DiscordId == context.Channel.Id);

            var guildEvent = new Models.Event()
            {
                Name = name,
                Description = description,
                DateTime = dateTime,
                Message = new Message()
                {
                    DiscordId = message.Id,
                }
            };

            if (channel == null)
            {
                guildEvent.Message.Channel = new Channel()
                {
                    DiscordId = context.Channel.Id
                };
            }
            else
            {
                guildEvent.Message.ChannelId = channel.Id;
            }

            await m_dbContext.SaveChangesAsync();

            var eventMessage = new EventMessage(context.Client, message, guildEvent);

            await eventMessage.AddReactionsToEventMessageAsync();
        }
    }
}
