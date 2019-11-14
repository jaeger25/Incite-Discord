using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
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
    [Description("Commands for setting and retrieving info about the guild's schedule")]
    public class EventCommands : BaseCommandModule
    {
        [Command("create")]
        [RequireRoles(RoleCheckMode.Any, "officer")]
        [Description("Creates a raid event")]
        public async Task Create(CommandContext context, string name, DateTimeOffset date)
        {
            var message = await EventMessage.CreateEventMessageAsync(context, name, date);
        }
    }
}
