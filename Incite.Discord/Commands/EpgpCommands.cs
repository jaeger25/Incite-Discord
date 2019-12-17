using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Exceptions;
using DSharpPlus.Interactivity;
using Incite.Discord.ApiModels;
using Incite.Discord.Attributes;
using Incite.Discord.Extensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Commands
{
    [Group("epgp")]
    [RequireGuild]
    [ModuleLifespan(ModuleLifespan.Transient)]
    [Description("Commands for managing EPGP")]
    public class EpgpCommands : BaseInciteCommand
    {
        readonly InciteDbContext m_dbContext;

        public EpgpCommands(InciteDbContext dbContext)
        {
            m_dbContext = dbContext;
        }

        [Command("list")]
        [RequireInciteRole(RoleKind.Member)]
        [Description("Lists the guild member's EPGP")]
        public async Task List(CommandContext context)
        {
        }
    }
}
