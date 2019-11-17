﻿// <auto-generated />
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Incite.Migrations
{
    [DbContext(typeof(InciteDbContext))]
    partial class InciteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.0-preview3.19554.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Incite.Models.Channel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("GuildId")
                        .HasColumnType("int");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.HasIndex("GuildId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Incite.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Incite.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("GuildId")
                        .HasColumnType("int");

                    b.Property<string>("PrimaryCharacterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.HasIndex("GuildId");

                    b.HasIndex("RoleId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Incite.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int>("GuildId")
                        .HasColumnType("int");

                    b.Property<int>("Kind")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.HasIndex("GuildId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Incite.Models.WowClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WowClasses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Warrior"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Rogue"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Hunter"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Mage"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Warlock"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Priest"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Druid"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Shaman"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Paladin"
                        });
                });

            modelBuilder.Entity("Incite.Models.WowProfession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WowProfessions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "First Aid"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Fishing"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Cooking"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Alchemy"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Blacksmithing"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Enchanting"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Engineering"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Leatherworking"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Tailoring"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Herbalism"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Mining"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Skinning"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Lockpicking"
                        });
                });

            modelBuilder.Entity("Incite.Models.Channel", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.Member", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incite.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.Role", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany()
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
