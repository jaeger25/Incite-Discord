using Castle.Core.Logging;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Incite.Discord.Converters;
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
using static DSharpPlus.CommandsNext.CommandsNextExtension;

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
                Services = serviceProvider,
            });

            commands.RegisterConverter(new EnumConverter<RoleKind>());
            commands.RegisterConverter(new EnumConverter<ChannelKind>());
            commands.RegisterConverter(new EnumConverter<WowFaction>());
            commands.RegisterCommands(GetType().Assembly);

            commands.RegisterConverter(new QualifiedWowCharacterConverter());
            commands.RegisterConverter(new UserWowCharacterConverter());
            commands.RegisterConverter(new WowCharacterConverter());
            commands.RegisterConverter(new WowClassConverter());
            commands.RegisterConverter(new WowItemConverter());
            commands.RegisterConverter(new WowItemsConverter());
            commands.RegisterConverter(new WowItemRecipeConverter());
            commands.RegisterConverter(new WowProfessionConverter());
            commands.RegisterConverter(new WowServerConverter());

            client.AddExtension(new HandlersExtension());
            client.GetExtension<HandlersExtension>().RegisterHandlers(typeof(Program).Assembly);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            m_discordClient.Ready += DiscordClient_Ready;

            var commands = m_discordClient.GetCommandsNext();
            commands.CommandExecuted += Commands_CommandExecuted;
            commands.CommandErrored += Commands_CommandErrored;

            await m_discordClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await m_discordClient.DisconnectAsync();

            var commands = m_discordClient.GetCommandsNext();
            commands.CommandErrored -= Commands_CommandErrored;
            commands.CommandExecuted -= Commands_CommandExecuted;

            m_discordClient.Ready -= DiscordClient_Ready;
        }

        async Task DiscordClient_Ready(ReadyEventArgs e)
        {
            await e.Client.UpdateStatusAsync(new DiscordActivity("for !help command", ActivityType.Watching));
        }

        async Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            m_logger.LogError($"CommandErrored: {e.Command?.QualifiedName}\nUser: {e.Context?.User}\n{e?.Exception}");

            if (e.Command?.Name != "help")
            {
                var helpCommand = e.Context.CommandsNext.FindCommand($"help {e.Command?.QualifiedName}", out string rawArgs);
                var helpContext = e.Context.CommandsNext.CreateContext(e.Context.Message, "!", helpCommand, rawArgs);

                await e.Context.CommandsNext.ExecuteCommandAsync(helpContext);
            }
        }

        Task Commands_CommandExecuted(CommandExecutionEventArgs e)
        {
            m_logger.LogInformation($"CommandExecuted: {e.Command?.QualifiedName}\nUser: {e.Context?.User}");
            return Task.CompletedTask;
        }
    }
}
