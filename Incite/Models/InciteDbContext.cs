﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Incite.Models
{
    public class InciteDbContext : DbContext
    {
        public InciteDbContext() : base()
        {
        }

        public InciteDbContext(DbContextOptions<InciteDbContext> options) : base(options)
        {
        }

        public DbSet<Guild> Guilds { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<WowClass> WowClasses { get; set; }
        public DbSet<WowProfession> WowProfessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            WowClass.Seed(modelBuilder);
            WowProfession.Seed(modelBuilder);
        }
    }
}
