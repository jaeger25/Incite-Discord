using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Incite.DbProviderMigrator
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets("894aa8f3-8db9-4868-8964-a973da7e7cc8")
                .AddEnvironmentVariables()
                .Build();

            using var sqlServerDbContext = new InciteDbContext(new DbContextOptionsBuilder<InciteDbContext>()
                .UseSqlServer(config["ConnectionStrings:SqlServer"], sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }).Options);

            using var postgresDbContext = new InciteDbContext(new DbContextOptionsBuilder<InciteDbContext>()
                .UseNpgsql(config["ConnectionStrings:Postgres"], sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                }).Options);

            postgresDbContext.Database.MigrateAsync().Wait();

            postgresDbContext.WowItemClasses.AddRange(sqlServerDbContext.WowItemClasses);
            postgresDbContext.WowItemSubclasses.AddRange(sqlServerDbContext.WowItemSubclasses);
            postgresDbContext.WowItems.AddRange(sqlServerDbContext.WowItems);
            postgresDbContext.WowSpells.AddRange(sqlServerDbContext.WowSpells);

            postgresDbContext.Users.AddRange(sqlServerDbContext.Users);
            postgresDbContext.Guilds.AddRange(sqlServerDbContext.Guilds);
            postgresDbContext.Roles.AddRange(sqlServerDbContext.Roles);

            postgresDbContext.WowCharacters.AddRange(sqlServerDbContext.WowCharacters);
            postgresDbContext.WowCharacterProfessions.AddRange(sqlServerDbContext.WowCharacterProfessions);

            postgresDbContext.Channels.AddRange(sqlServerDbContext.Channels);
            postgresDbContext.Messages.AddRange(sqlServerDbContext.Messages);

            postgresDbContext.Members.AddRange(sqlServerDbContext.Members);
            postgresDbContext.Events.AddRange(sqlServerDbContext.Events);

            postgresDbContext.SaveChanges();
        }
    }
}
