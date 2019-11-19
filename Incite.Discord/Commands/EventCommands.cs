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
            var message = await context.Message.RespondAsync("\u200b");
            await context.Message.DeleteAsync();

            var guild = await m_dbContext.Guilds
                .FirstAsync(x => x.DiscordId == context.Guild.Id);

            var channel = await m_dbContext.Channels
                .FirstOrDefaultAsync(x => x.DiscordId == context.Channel.Id);

            var guildEvent = new Models.Event()
            {
                Name = name,
                Description = description,
                DateTime = dateTime,
                GuildId = guild.Id,
                Message = new Message()
                {
                    DiscordId = message.Id,
                }
            };

            if (channel == null)
            {
                guildEvent.Message.Channel = new Channel()
                {
                    DiscordId = context.Channel.Id,
                    GuildId = guild.Id
                };
            }
            else
            {
                guildEvent.Message.ChannelId = channel.Id;
            }

            guild.Events.Add(guildEvent);
            await m_dbContext.SaveChangesAsync();

            var eventMessage = new EventMessage(context.Client, message, guildEvent);
            await eventMessage.UpdateAsync(guildEvent);

            await eventMessage.AddReactionsToEventMessageAsync();
        }
    }
}
