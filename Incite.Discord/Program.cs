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
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace Incite.Discord
{
    class InciteDiscordOptions
    {
        public const string Environment_Production = "Production";
        public const string Environment_Development = "Development";

        public string Environment { get; set; }
    }

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

                    services.Configure<InciteDiscordOptions>(config);

                    bool isProduction =
                        config.Get<InciteDiscordOptions>().Environment == InciteDiscordOptions.Environment_Production;

                    var discordClient = new DiscordClient(new DiscordConfiguration
                    {
                        AutoReconnect = true,
                        Token = config["Discord:BotToken"],
                        TokenType = TokenType.Bot,
                        UseInternalLogHandler = true,
                        LogLevel = isProduction ?
                            DSharpPlus.LogLevel.Warning : DSharpPlus.LogLevel.Info,
                    });

                    services.AddEntityFrameworkNpgsql()
                        .AddSingleton<IConfiguration>(config)
                        .AddSingleton<DiscordClient>(discordClient)
                        .AddSingleton<EmojiService>()
                        .AddSingleton<GuildCommandPrefixCache>()
                        .AddScoped<WowHeadService>()
                        .AddDbContext<InciteDbContext>(options =>
                        {
                            options.UseLazyLoadingProxies()
                                .UseNpgsql(config["ConnectionStrings:Postgres"], sqlOptions =>
                                {
                                    sqlOptions.EnableRetryOnFailure();
                                });
                        })
                        .AddLogging(builder =>
                        {
                            builder.AddFilter("Microsoft", Microsoft.Extensions.Logging.LogLevel.Warning)
                                .AddFilter("System", Microsoft.Extensions.Logging.LogLevel.Warning)
                                .AddFilter("REST", Microsoft.Extensions.Logging.LogLevel.Error)
                                .SetMinimumLevel(isProduction ? Microsoft.Extensions.Logging.LogLevel.Warning : Microsoft.Extensions.Logging.LogLevel.Information)
                                .AddConsole();
                        });

                    services.AddHostedService<DiscordService>();
                });

        static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Services.GetService<InciteDbContext>().Database.MigrateAsync().Wait();
            host.Run();
        }
    }
}
