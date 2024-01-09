﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SwsCareer.Database.Sqlite;

#nullable disable

namespace Swos.CareerMod.Database.Sqlite.Migrations
{
    [DbContext(typeof(SqliteCareerModDbContext))]
    [Migration("20240109190739_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("Swos.CareerMod.Database.Models.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<byte>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BonusRating")
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

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<byte>("YouthRating")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Swos.CareerMod.Database.Models.PlayerSkill", b =>
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

                    b.ToTable("PlayerSkills");
                });

            modelBuilder.Entity("Swos.CareerMod.Database.Models.PlayerSkill", b =>
                {
                    b.HasOne("Swos.CareerMod.Database.Models.Player", null)
                        .WithMany("Skills")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Swos.CareerMod.Database.Models.Player", b =>
                {
                    b.Navigation("Skills");
                });
#pragma warning restore 612, 618
        }
    }
}
