using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Converters;
using DSharpPlus.Entities;
using Incite.Discord.DiscordExtensions;
using Incite.Discord.Handlers;
using Incite.Discord.Services;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;

namespace Incite.Discord
{
    class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile("appsettings.development.json", true)
                        .AddUserSecrets("97f50552-3f23-43f2-af0f-ab150fb1bcdb")
                        .AddEnvironmentVariables()
                        .Build();

                    var discordClient = new DiscordClient(new DiscordConfiguration
                    {
                        AutoReconnect = true,
                        LogLevel = LogLevel.Debug,
                        Token = config["Discord:BotToken"],
                        TokenType = TokenType.Bot,
                        UseInternalLogHandler = true,
                    });

                    services.AddEntityFrameworkSqlServer()
                        .AddSingleton<IConfiguration>(config)
                        .AddSingleton<DiscordClient>(discordClient)
                        .AddSingleton<EmojiService>()
                        .AddDbContextPool<InciteDbContext>(options =>
                        {
                            options.UseLazyLoadingProxies()
                                .UseSqlServer(config["ConnectionStrings:Default"]);
                        });

                    services.AddHostedService<DiscordService>();
                });

        static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}
