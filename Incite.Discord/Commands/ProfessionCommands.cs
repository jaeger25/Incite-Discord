using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using Incite.Discord.Attributes;
using Incite.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Discord.Commands
{
    [Group("profession")]
    [RequireGuildConfigured]
    [Description("Commands for managing guild members")]
    public class ProfessionCommands : BaseCommandModule
    {
        readonly InciteDbContext m_dbContext;

        public ProfessionCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }
    }
}
