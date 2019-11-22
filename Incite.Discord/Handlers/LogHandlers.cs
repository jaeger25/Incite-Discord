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
        readonly ILogger m_logger;

        public LogHandlers(DiscordClient client, ILogger logger)
        {
            m_logger = logger;

            var commands = client.GetCommandsNext();
            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;
        }

        private Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            m_logger.LogError($"CommandErrored: {e.Command.Name}\nParams: {e.Context.RawArguments}\n{e.Exception.Message}");
            return Task.FromResult(0);
        }

        private Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            m_logger.LogInformation($"CommandExecuted: {e.Command.Name}\nParams: {e.Context.RawArguments}");
            return Task.FromResult(0);
        }
    }
}
