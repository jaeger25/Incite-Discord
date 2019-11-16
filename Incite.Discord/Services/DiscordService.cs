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
        DiscordClient Client { get; }

        public DiscordService(IConfiguration config, IServiceProvider provider)
        {
            Client = new DiscordClient(new DiscordConfiguration
            {
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                Token = config["Discord:BotToken"],
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
            });

            var commands = Client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
                EnableDms = false,
                DmHelp = true,
                Services = provider
            });

            commands.RegisterConverter(new EnumConverter<RoleKind>());
            commands.RegisterConverter(new EnumConverter<ChannelKind>());
            commands.RegisterCommands(GetType().Assembly);

            Client.AddExtension(new HandlersExtension());
            Client.AddExtension(new DatabaseExtension(config));

            Client.GetExtension<HandlersExtension>().RegisterHandlers(typeof(Program).Assembly);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var commands = Client.GetCommandsNext();
            commands.CommandErrored += Commands_CommandErrored;

            await Client.ConnectAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Client?.DisconnectAsync();

            var commands = Client?.GetCommandsNext();
            if (commands == null)
            {
                commands.CommandErrored -= Commands_CommandErrored;
            }
        }

        private Task Commands_CommandErrored(CommandErrorEventArgs e)
        {
            return Task.FromResult(0);
        }
    }
}
