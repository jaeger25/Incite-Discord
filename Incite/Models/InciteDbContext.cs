﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<Channel> Channels { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventMember> EventMembers { get; set; }
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberEvent> MemberEvents { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WowCharacter> WowCharacters { get; set; }
        public DbSet<WowCharacterProfession> WowCharacterProfessions { get; set; }
        public DbSet<WowClass> WowClasses { get; set; }
        public DbSet<WowProfession> WowProfessions { get; set; }
        public DbSet<WowServer> WowServers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Channel>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Channels)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Events)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Event>()
                .OwnsOne(x => x.EventMessage)
                    .WithOwner(x => x.Event);

            modelBuilder.Entity<Event>()
                .OwnsMany(x => x.EventMembers)
                    .WithOwner(x => x.Event);

            modelBuilder.Entity<Guild>()
                .HasAlternateKey(x => x.DiscordId);

            modelBuilder.Entity<Member>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Members)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Member>()
                .OwnsMany(x => x.MemberEvents)
                    .WithOwner(x => x.Member);

            modelBuilder.Entity<Message>()
                .HasOne(x => x.Channel)
                    .WithMany(x => x.Messages)
                .HasForeignKey(x => x.ChannelId)
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
                .OwnsMany(x => x.WowCharacterProfessions)
                    .WithOwner(x => x.WowCharacter);

            WowClass.Seed(modelBuilder);
            WowProfession.Seed(modelBuilder);
            WowServer.Seed(modelBuilder);
        }
    }
}
