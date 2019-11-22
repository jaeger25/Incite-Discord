using Castle.Core.Logging;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incite.Discord.Services
{
    public class DiscordService : IHostedService
    {
        readonly DiscordClient m_discordClient;
        readonly ILogger<DiscordService> m_logger;

        public DiscordService(DiscordClient client, IServiceProvider serviceProvider, ILogger<DiscordService> logger)
        {
            m_discordClient = client;
            m_logger = logger;

            var commands = m_discordClient.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
                EnableDms = false,
                Services = serviceProvider
            });

            commands.RegisterConverter(new EnumConverter<RoleKind>());
            commands.RegisterConverter(new EnumConverter<ChannelKind>());
            commands.RegisterCommands(GetType().Assembly);

            client.AddExtension(new HandlersExtension());
            client.GetExtension<HandlersExtension>().RegisterHandlers(typeof(Program).Assembly);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var commands = m_discordClient.GetCommandsNext();
            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;

            await m_discordClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await m_discordClient.DisconnectAsync();

            m_discordClient.GetCommandsNext().CommandErrored -= Commands_CommandErrored;
        }

        Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            m_logger.LogError($"CommandErrored: {e.Command.Name}\nParams: {e.Context.RawArguments?.Aggregate((x, y) => x + " " + y)}\n{e.Exception.Message}");
            return Task.CompletedTask;
        }

        Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            m_logger.LogInformation($"CommandExecuted: {e.Command.Name}\nParams: {e.Context.RawArguments?.Aggregate((x, y) => x + " " + y)}");
            return Task.CompletedTask;
        }
    }
}
