using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class InciteDbContext : DbContext
    {
        public InciteDbContext(DbContextOptions<InciteDbContext> options) : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WowCharacter> WowCharacters { get; set; }
        public DbSet<WowCharacterProfession> WowCharacterProfessions { get; set; }
        public DbSet<WowCharacterRecipe> WowCharacterRecipes { get; set; }
        public DbSet<WowClass> WowClasses { get; set; }
        public DbSet<WowItem> WowItems { get; set; }
        public DbSet<WowItemClass> WowItemClasses { get; set; }
        public DbSet<WowItemSubclass> WowItemSubclasses { get; set; }
        public DbSet<WowProfession> WowProfessions { get; set; }
        public DbSet<WowReagent> WowReagents { get; set; }
        public DbSet<WowServer> WowServers { get; set; }
        public DbSet<WowSpell> WowSpells { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Events)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .OwnsOne(x => x.EventMessage)
                    .WithOwner();

            modelBuilder.Entity<Event>()
                .OwnsMany(x => x.EventMembers)
                    .WithOwner(x => x.Event);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Owner)
                    .WithMany(x => x.OwnedEvents);

            modelBuilder.Entity<Guild>()
                .HasAlternateKey(x => x.DiscordId);

            modelBuilder.Entity<Member>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Members)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Roles)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasAlternateKey(x => x.DiscordId);

            modelBuilder.Entity<WowCharacter>()
                .HasOne(x => x.User)
                    .WithMany(x => x.WowCharacters)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WowCharacter>()
                .HasOne(x => x.WowClass)
                    .WithMany(x => x.WowCharacters)
                .HasForeignKey(x => x.WowClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WowCharacter>()
                .HasOne(x => x.WowServer)
                    .WithMany(x => x.WowCharacters)
                .HasForeignKey(x => x.WowServerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WowCharacter>()
                .HasMany(x => x.WowCharacterProfessions)
                    .WithOne(x => x.WowCharacter);

            modelBuilder.Entity<WowCharacterProfession>()
                .OwnsMany(x => x.WowCharacterRecipes)
                    .WithOwner(x => x.WowCharacterProfession);

            modelBuilder.Entity<WowItem>()
                .HasOne(x => x.WowItemClass)
                    .WithMany(x => x.WowItems)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WowItem>()
                .HasOne(x => x.WowItemSubclass)
                    .WithMany(x => x.WowItems)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WowItemClass>()
                .HasMany(x => x.WowItemSubclasses)
                    .WithOne(x => x.WowItemClass)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WowItemSubclass>()
                .HasOne(x => x.WowItemClass)
                    .WithMany(x => x.WowItemSubclasses)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WowSpell>()
                .OwnsMany(x => x.WowReagents)
                    .WithOwner(x => x.WowSpell);

            WowClass.Seed(modelBuilder);
            WowProfession.Seed(modelBuilder);
            WowServer.Seed(modelBuilder);
        }
    }
}
