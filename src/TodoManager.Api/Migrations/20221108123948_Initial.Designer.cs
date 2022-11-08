﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoManager.Api.Data;

#nullable disable

namespace TodoManager.Api.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20221108123948_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.10");

            modelBuilder.Entity("TodoManager.Api.Model.TodoTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Tags");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Tag1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Tag2"
                        });
                });

            modelBuilder.Entity("TodoManager.Api.Model.TodoTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Created")
                        .HasColumnType("Date");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ParentTaskId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ParentTaskId");

                    b.ToTable("Tasks");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8553),
                            Description = "d1",
                            Level = 0,
                            Name = "n1"
                        },
                        new
                        {
                            Id = 2,
                            Created = new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8569),
                            Description = "d2",
                            Level = 1,
                            Name = "n2",
                            ParentTaskId = 1
                        },
                        new
                        {
                            Id = 3,
                            Created = new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8571),
                            Description = "d3",
                            Level = 2,
                            Name = "n3",
                            ParentTaskId = 2
                        },
                        new
                        {
                            Id = 4,
                            Created = new DateTime(2022, 11, 8, 17, 39, 48, 490, DateTimeKind.Local).AddTicks(8572),
                            Description = "d4",
                            Level = 3,
                            Name = "n4",
                            ParentTaskId = 3
                        });
                });

            modelBuilder.Entity("TodoTagTodoTask", b =>
                {
                    b.Property<int>("TagsId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TasksId")
                        .HasColumnType("INTEGER");

                    b.HasKey("TagsId", "TasksId");

                    b.HasIndex("TasksId");

                    b.ToTable("TodoTagTodoTask");

                    b.HasData(
                        new
                        {
                            TagsId = 1,
                            TasksId = 1
                        },
                        new
                        {
                            TagsId = 2,
                            TasksId = 1
                        },
                        new
                        {
                            TagsId = 2,
                            TasksId = 2
                        },
                        new
                        {
                            TagsId = 1,
                            TasksId = 3
                        },
                        new
                        {
                            TagsId = 2,
                            TasksId = 4
                        });
                });

            modelBuilder.Entity("TodoManager.Api.Model.TodoTask", b =>
                {
                    b.HasOne("TodoManager.Api.Model.TodoTask", "ParentTask")
                        .WithMany("SubTasks")
                        .HasForeignKey("ParentTaskId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("ParentTask");
                });

            modelBuilder.Entity("TodoTagTodoTask", b =>
                {
                    b.HasOne("TodoManager.Api.Model.TodoTag", null)
                        .WithMany()
                        .HasForeignKey("TagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TodoManager.Api.Model.TodoTask", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TodoManager.Api.Model.TodoTask", b =>
                {
                    b.Navigation("SubTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
