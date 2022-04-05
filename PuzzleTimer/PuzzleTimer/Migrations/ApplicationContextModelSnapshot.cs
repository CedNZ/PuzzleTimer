﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PuzzleTimer.Models;

#nullable disable

namespace PuzzleTimer.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0-preview.2.22153.1");

            modelBuilder.Entity("PuzzleTimer.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileName")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PuzzleId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SolvingSessionId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PuzzleId");

                    b.HasIndex("SolvingSessionId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("PuzzleTimer.Models.Puzzle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("PieceCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Puzzles");
                });

            modelBuilder.Entity("PuzzleTimer.Models.SolvingSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Completed")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PuzzleId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Started")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PuzzleId");

                    b.ToTable("SolvingSessions");
                });

            modelBuilder.Entity("PuzzleTimer.Models.TimeEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Comment")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("SolvingSessionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int?>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SolvingSessionId");

                    b.HasIndex("UserId");

                    b.ToTable("TimeEntries");
                });

            modelBuilder.Entity("PuzzleTimer.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SolvingSessionUser", b =>
                {
                    b.Property<int>("SolvingSessionsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UsersId")
                        .HasColumnType("INTEGER");

                    b.HasKey("SolvingSessionsId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("SolvingSessionUser");
                });

            modelBuilder.Entity("PuzzleTimer.Models.Image", b =>
                {
                    b.HasOne("PuzzleTimer.Models.Puzzle", "Puzzle")
                        .WithMany()
                        .HasForeignKey("PuzzleId");

                    b.HasOne("PuzzleTimer.Models.SolvingSession", "SolvingSession")
                        .WithMany()
                        .HasForeignKey("SolvingSessionId");

                    b.Navigation("Puzzle");

                    b.Navigation("SolvingSession");
                });

            modelBuilder.Entity("PuzzleTimer.Models.SolvingSession", b =>
                {
                    b.HasOne("PuzzleTimer.Models.Puzzle", "Puzzle")
                        .WithMany()
                        .HasForeignKey("PuzzleId");

                    b.Navigation("Puzzle");
                });

            modelBuilder.Entity("PuzzleTimer.Models.TimeEntry", b =>
                {
                    b.HasOne("PuzzleTimer.Models.SolvingSession", "SolvingSession")
                        .WithMany("TimeEntries")
                        .HasForeignKey("SolvingSessionId");

                    b.HasOne("PuzzleTimer.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("SolvingSession");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SolvingSessionUser", b =>
                {
                    b.HasOne("PuzzleTimer.Models.SolvingSession", null)
                        .WithMany()
                        .HasForeignKey("SolvingSessionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PuzzleTimer.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PuzzleTimer.Models.SolvingSession", b =>
                {
                    b.Navigation("TimeEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
