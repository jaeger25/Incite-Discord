using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Discord.Messages;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("event")]
    [RequireGuildConfigured]
    [Description("Commands for setting and retrieving info about the guild's schedule")]
    public class EventCommands : BaseCommandModule
    {
        [Command("create")]
        [RequirePermissions(Permissions.SendMessages)]
        [Description("Creates an event")]
        public async Task Create(CommandContext context,
            string name,
            [Description("Format: \"10-31-2019 9:00 PM\"")] DateTimeOffset date)
        {
            await EventMessage.CreateEventMessageAsync(context, name, date);
        }
    }
}
