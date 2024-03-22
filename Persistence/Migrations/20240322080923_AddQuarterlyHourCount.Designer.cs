﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PipelineApp2._0.Persistence;

#nullable disable

namespace PipelineApp2._0.Migrations
{
    [DbContext(typeof(PipelineDbContext))]
    [Migration("20240322080923_AddQuarterlyHourCount")]
    partial class AddQuarterlyHourCount
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.17");

            modelBuilder.Entity("PipelineApp2._0.Domain.DateEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("DateEntries");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.QuarterlyHourCount", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Hours")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("QuarterlyHours");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.Setting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("AddLunchBreaks")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LunchBreakInMinutes")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.Tag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Colour")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SettingId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SettingId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.WeekDay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Hours")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsWorkDay")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Minutes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("SettingId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SettingId");

                    b.ToTable("WeekDays");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.Tag", b =>
                {
                    b.HasOne("PipelineApp2._0.Domain.Setting", null)
                        .WithMany("Tags")
                        .HasForeignKey("SettingId");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.WeekDay", b =>
                {
                    b.HasOne("PipelineApp2._0.Domain.Setting", null)
                        .WithMany("WeekDays")
                        .HasForeignKey("SettingId");
                });

            modelBuilder.Entity("PipelineApp2._0.Domain.Setting", b =>
                {
                    b.Navigation("Tags");

                    b.Navigation("WeekDays");
                });
#pragma warning restore 612, 618
        }
    }
}
