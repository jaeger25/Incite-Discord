using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using Incite.Discord.DiscordExtensions;
using Incite.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incite.Discord.Services
{
    public class DiscordService : IHostedService
    {
        readonly DiscordClient m_discordClient;

        public DiscordService(DiscordClient client, IServiceProvider serviceProvider)
        {
            m_discordClient = client;

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
            commands.CommandErrored += Commands_CommandErrored;

            await m_discordClient.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await m_discordClient.DisconnectAsync();

            m_discordClient.GetCommandsNext().CommandErrored -= Commands_CommandErrored;
        }

        private Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            return Task.FromResult(0);
        }
    }
}
