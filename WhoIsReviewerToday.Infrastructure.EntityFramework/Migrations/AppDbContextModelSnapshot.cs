﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using WhoIsReviewerToday.Infrastructure.EntityFramework.DbContext;

namespace WhoIsReviewerToday.Infrastructure.EntityFramework.Migrations
{
    [DbContext(typeof(AppDbContext))]
    internal class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity(
                "WhoIsReviewerToday.Domain.Models.Chat",
                b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FullName");

                    b.Property<long>("TelegramChatId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Chats");
                });

            modelBuilder.Entity(
                "WhoIsReviewerToday.Domain.Models.Developer",
                b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long?>("ChatId");

                    b.Property<string>("FullName");

                    b.Property<int>("Team");

                    b.Property<long?>("TelegramUserId");

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.ToTable("Developers");
                });

            modelBuilder.Entity(
                "WhoIsReviewerToday.Domain.Models.Review",
                b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateTime");

                    b.Property<long>("DeveloperId");

                    b.HasKey("Id");

                    b.HasIndex("DeveloperId");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity(
                "WhoIsReviewerToday.Domain.Models.Developer",
                b =>
                {
                    b.HasOne("WhoIsReviewerToday.Domain.Models.Chat", "Chat")
                        .WithMany()
                        .HasForeignKey("ChatId");
                });

            modelBuilder.Entity(
                "WhoIsReviewerToday.Domain.Models.Review",
                b =>
                {
                    b.HasOne("WhoIsReviewerToday.Domain.Models.Developer", "Developer")
                        .WithMany()
                        .HasForeignKey("DeveloperId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}