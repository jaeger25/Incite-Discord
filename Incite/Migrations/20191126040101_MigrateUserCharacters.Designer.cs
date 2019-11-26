﻿// <auto-generated />
using System;
using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Incite.Migrations
{
    [DbContext(typeof(InciteDbContext))]
    [Migration("20191126040101_MigrateUserCharacters")]
    partial class MigrateUserCharacters
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.HasIndex("GuildId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("Incite.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("DateTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EventMessageId")
                        .HasColumnType("int");

                    b.Property<int>("GuildId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Incite.Models.Guild", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.Property<int?>("WowServerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.HasIndex("WowServerId");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("Incite.Models.Member", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GuildId")
                        .HasColumnType("int");

                    b.Property<string>("PrimaryCharacterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.HasIndex("UserId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("Incite.Models.MemberRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MemberId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("RoleId");

                    b.ToTable("MemberRoles");
                });

            modelBuilder.Entity("Incite.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChannelId")
                        .HasColumnType("int");

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.ToTable("Messages");
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

                    b.HasIndex("GuildId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Incite.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("DiscordId")
                        .HasColumnType("decimal(20,0)");

                    b.HasKey("Id");

                    b.HasAlternateKey("DiscordId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Incite.Models.WowCharacter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("WowClassId")
                        .HasColumnType("int");

                    b.Property<int>("WowServerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("WowClassId");

                    b.HasIndex("WowServerId");

                    b.ToTable("WowCharacters");
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

            modelBuilder.Entity("Incite.Models.WowServer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WowServers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Kirtonos"
                        });
                });

            modelBuilder.Entity("Incite.Models.Channel", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany("Channels")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.Event", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany("Events")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.OwnsMany("Incite.Models.EventMember", "EventMembers", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<string>("EmojiDiscordName")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("EventId")
                                .HasColumnType("int");

                            b1.Property<int>("MemberId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("EventId");

                            b1.HasIndex("MemberId");

                            b1.ToTable("EventMembers");

                            b1.WithOwner("Event")
                                .HasForeignKey("EventId");

                            b1.HasOne("Incite.Models.Member", "Member")
                                .WithMany()
                                .HasForeignKey("MemberId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });

                    b.OwnsOne("Incite.Models.EventMessage", "EventMessage", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("EventId")
                                .HasColumnType("int");

                            b1.Property<int>("MessageId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("EventId")
                                .IsUnique();

                            b1.HasIndex("MessageId");

                            b1.ToTable("Events1");

                            b1.WithOwner("Event")
                                .HasForeignKey("EventId");

                            b1.HasOne("Incite.Models.Message", "Message")
                                .WithMany()
                                .HasForeignKey("MessageId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();
                        });
                });

            modelBuilder.Entity("Incite.Models.Guild", b =>
                {
                    b.HasOne("Incite.Models.WowServer", "WowServer")
                        .WithMany()
                        .HasForeignKey("WowServerId");
                });

            modelBuilder.Entity("Incite.Models.Member", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany("Members")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Incite.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("Incite.Models.MemberEvent", "MemberEvents", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("EventId")
                                .HasColumnType("int");

                            b1.Property<int>("MemberId")
                                .HasColumnType("int");

                            b1.HasKey("Id");

                            b1.HasIndex("EventId");

                            b1.HasIndex("MemberId");

                            b1.ToTable("MemberEvents");

                            b1.HasOne("Incite.Models.Event", "Event")
                                .WithMany()
                                .HasForeignKey("EventId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .IsRequired();

                            b1.WithOwner("Member")
                                .HasForeignKey("MemberId");
                        });
                });

            modelBuilder.Entity("Incite.Models.MemberRole", b =>
                {
                    b.HasOne("Incite.Models.Member", "Member")
                        .WithMany("MemberRoles")
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incite.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.Message", b =>
                {
                    b.HasOne("Incite.Models.Channel", "Channel")
                        .WithMany("Messages")
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.Role", b =>
                {
                    b.HasOne("Incite.Models.Guild", "Guild")
                        .WithMany("Roles")
                        .HasForeignKey("GuildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("Incite.Models.WowCharacter", b =>
                {
                    b.HasOne("Incite.Models.User", "User")
                        .WithMany("WowCharacters")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incite.Models.WowClass", "WowClass")
                        .WithMany("WowCharacters")
                        .HasForeignKey("WowClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Incite.Models.WowServer", "WowServer")
                        .WithMany("WowCharacters")
                        .HasForeignKey("WowServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
