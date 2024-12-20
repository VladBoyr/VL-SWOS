﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Swos.Database.Sqlite;

#nullable disable

namespace Swos.Database.Sqlite.Migrations
{
    [DbContext(typeof(SwosDbContextSqlite))]
    partial class SwosDbContextSqliteModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("Swos.Database.Models.DbSwosKit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("KitType")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ShirtExtraColor")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ShirtMainColor")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("ShortsColor")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SocksColor")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TeamKits");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Country")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Face")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Number")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Position")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Rating")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosSkill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("PrimarySkill")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Skill")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("SkillValue")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AwayKitId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("CoachName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Country")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Division")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("GlobalId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HomeKitId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocalId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("Tactic")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamDatabaseId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AwayKitId");

                    b.HasIndex("HomeKitId");

                    b.HasIndex("TeamDatabaseId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosTeamPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<byte>("PlayerPositionIndex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("TeamId");

                    b.ToTable("TeamPlayers");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalPlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GlobalPlayers");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalPlayerSwos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GlobalPlayerId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SwosPlayerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GlobalPlayerId");

                    b.HasIndex("SwosPlayerId");

                    b.ToTable("GlobalPlayerSwos");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalTeam", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("GlobalTeams");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalTeamSwos", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GlobalTeamId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SwosTeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GlobalTeamId");

                    b.HasIndex("SwosTeamId");

                    b.ToTable("GlobalTeamSwos");
                });

            modelBuilder.Entity("Swos.Database.Models.TeamDatabase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Version")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TeamDatabases");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosSkill", b =>
                {
                    b.HasOne("Swos.Database.Models.DbSwosPlayer", null)
                        .WithMany("Skills")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosTeam", b =>
                {
                    b.HasOne("Swos.Database.Models.DbSwosKit", "AwayKit")
                        .WithMany()
                        .HasForeignKey("AwayKitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swos.Database.Models.DbSwosKit", "HomeKit")
                        .WithMany()
                        .HasForeignKey("HomeKitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swos.Database.Models.TeamDatabase", null)
                        .WithMany("Teams")
                        .HasForeignKey("TeamDatabaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AwayKit");

                    b.Navigation("HomeKit");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosTeamPlayer", b =>
                {
                    b.HasOne("Swos.Database.Models.DbSwosPlayer", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swos.Database.Models.DbSwosTeam", null)
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalPlayerSwos", b =>
                {
                    b.HasOne("Swos.Database.Models.GlobalPlayer", null)
                        .WithMany("SwosPlayers")
                        .HasForeignKey("GlobalPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swos.Database.Models.DbSwosPlayer", "SwosPlayer")
                        .WithMany()
                        .HasForeignKey("SwosPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SwosPlayer");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalTeamSwos", b =>
                {
                    b.HasOne("Swos.Database.Models.GlobalTeam", null)
                        .WithMany("SwosTeams")
                        .HasForeignKey("GlobalTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Swos.Database.Models.DbSwosTeam", "SwosTeam")
                        .WithMany()
                        .HasForeignKey("SwosTeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SwosTeam");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosPlayer", b =>
                {
                    b.Navigation("Skills");
                });

            modelBuilder.Entity("Swos.Database.Models.DbSwosTeam", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalPlayer", b =>
                {
                    b.Navigation("SwosPlayers");
                });

            modelBuilder.Entity("Swos.Database.Models.GlobalTeam", b =>
                {
                    b.Navigation("SwosTeams");
                });

            modelBuilder.Entity("Swos.Database.Models.TeamDatabase", b =>
                {
                    b.Navigation("Teams");
                });
#pragma warning restore 612, 618
        }
    }
}
