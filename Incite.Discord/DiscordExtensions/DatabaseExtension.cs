using DSharpPlus;
using Incite.Discord.Handlers;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Incite.Discord.DiscordExtensions
{
    public class DatabaseExtension : BaseExtension
    {
        IConfiguration Config { get; }

        public DatabaseExtension(IConfiguration config)
        {
            Config = config;
        }

        protected override void Setup(DiscordClient client)
        {
            Client = client;
        }

        public InciteDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<InciteDbContext>()
                .UseInMemoryDatabase("Incite");

            return new InciteDbContext(options.Options);
        }
    }
}
