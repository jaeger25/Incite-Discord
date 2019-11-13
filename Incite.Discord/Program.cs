using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Handlers;
using Incite.Models;
using Microsoft.Extensions.Configuration;
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
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.development.json", true)
                .AddUserSecrets("97f50552-3f23-43f2-af0f-ab150fb1bcdb")
                .AddEnvironmentVariables()
                .Build();

            var client = new DiscordClient(new DiscordConfiguration
            {
                AutoReconnect = true,
                LogLevel = LogLevel.Debug,
                Token = config["Discord:BotToken"],
                TokenType = TokenType.Bot,
                UseInternalLogHandler = true,
            });

            var commands = client.UseCommandsNext(new CommandsNextConfiguration
            {
                StringPrefixes = new[] { "!" },
            });

            commands.RegisterCommands(typeof(Program).Assembly);

            client.AddExtension(new HandlersExtension());
            client.AddExtension(new DatabaseExtension(config));

            client.GetExtension<HandlersExtension>().RegisterHandlers(typeof(Program).Assembly);

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}
