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
using Microsoft.Extensions.Logging;
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
                        LogLevel = DSharpPlus.LogLevel.Debug,
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
                        })
                        .AddLogging(builder =>
                        {
                            builder.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning)
                                .AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning)
                                .AddConsole();
                        });

                    services.Configure<EventScheduledTaskOptions>(config.GetSection("EventScheduledTaskOptions"));

                    services.AddHostedService<DiscordService>();
                    //services.AddHostedService<EventScheduledTask>();
                });

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Services.GetService<InciteDbContext>().Database.MigrateAsync();
            host.Run();
        }
    }
}
