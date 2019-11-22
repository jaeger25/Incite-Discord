using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord.Handlers
{
    public class LogHandlers : BaseHandler
    {
        readonly ILogger<LogHandlers> m_logger;

        public LogHandlers(DiscordClient client, ILogger<LogHandlers> logger)
        {
            m_logger = logger;

            var commands = client.GetCommandsNext();
            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;
        }

        private Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            m_logger.LogError($"CommandErrored: {e.Command.Name}\nParams: {e.Context.RawArguments.Aggregate((x, y) => x + " " + y)}\n{e.Exception.Message}");
            return Task.FromResult(0);
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            m_logger.LogInformation($"CommandExecuted: {e.Command.Name}\nParams: {e.Context.RawArguments.Aggregate((x, y) => x + y)}");
            return Task.FromResult(0);
        }
    }
}
