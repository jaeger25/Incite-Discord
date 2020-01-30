using Incite.Models;
using Incite.NtService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Incite.NtService
{
    class InciteNtServiceOptions
    {
        public const string Environment_Production = "Production";
        public const string Environment_Development = "Development";

        public string Environment { get; set; }
    }

    class InciteService : ServiceControl
    {
        IHost m_host = null;

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", false)
                        .AddJsonFile("appsettings.development.json", true)
                        .AddUserSecrets("0021a7d4-5c77-450e-a852-71c077f1c81f")
                        .AddEnvironmentVariables()
                        .Build();

                    services.Configure<InciteNtServiceOptions>(config);

                    bool isProduction =
                        config.Get<InciteNtServiceOptions>().Environment == InciteNtServiceOptions.Environment_Production;

                    services
                        .AddSingleton<IConfiguration>(config)
                        .AddSingleton<WowAddOnService>()
                        .AddDbContext<InciteDbContext>(options =>
                        {
                            options.UseNpgsql(config["ConnectionStrings:Postgres"], sqlOptions =>
                                {
                                    sqlOptions.EnableRetryOnFailure();
                                });
                        })
                        .AddLogging(builder =>
                        {
                            builder.AddFilter("Microsoft", LogLevel.Warning)
                                .AddFilter("System", LogLevel.Warning)
                                .SetMinimumLevel(isProduction ? LogLevel.Warning : LogLevel.Information)
                                .AddConsole();
                        });
                });


        public bool Start(HostControl hostControl)
        {
            Task.Run(() =>
            {
                CreateHostBuilder(new string[] { })
                    .Build()
                    .Run();
            });

            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            m_host?.StopAsync().Wait();
            return true;
        }
    }
}
