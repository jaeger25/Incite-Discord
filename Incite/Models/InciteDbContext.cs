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
        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MemberRole> MemberRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<WowClass> WowClasses { get; set; }
        public DbSet<WowProfession> WowProfessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guild>()
                .HasAlternateKey(x => x.DiscordId);

            modelBuilder.Entity<User>()
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

            modelBuilder.Entity<Channel>()
                .HasOne(x => x.Guild)
                    .WithMany(x => x.Channels)
                .HasForeignKey(x => x.GuildId)
                .OnDelete(DeleteBehavior.Restrict);

            WowClass.Seed(modelBuilder);
            WowProfession.Seed(modelBuilder);
        }
    }
}
