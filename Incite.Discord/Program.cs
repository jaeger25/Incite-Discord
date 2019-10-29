using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Incite.Discord.Handlers;
using System;
using System.Threading.Tasks;

namespace Incite.Discord
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            var client = new DiscordClient(new DiscordConfiguration
            {
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                Token = "NjM3MDc3MDY2MDgwMTkwNDk4.XbJuwQ.C1GyxIxOqbKQZ1QW0Dsknhzjl2o",
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new []{ "!" },
            });

            commands.RegisterCommands(typeof(Program).Assembly);

            var handlers = new HandlersExtension();
            client.AddExtension(handlers);

            handlers.RegisterHandlers(typeof(Program).Assembly);

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
