using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("setup")]
    [Description("Commands for managing bot settings for this guild")]
    public class SetupCommands : BaseCommandModule
    {
        [Command("setrole")]
        [Description("Lists the official guild raid days and times")]
        public async Task SetRole(CommandContext context, InciteRole role, string roleName)
        {
        }
    }
}
