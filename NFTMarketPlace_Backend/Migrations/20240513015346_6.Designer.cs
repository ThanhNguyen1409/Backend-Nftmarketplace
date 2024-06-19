﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NFTMarketPlace_Backend.Data;

#nullable disable

namespace NFTMarketPlace_Backend.Migrations
{
    [DbContext(typeof(DbContextCRUD))]
    [Migration("20240513015346_6")]
    partial class _6
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NFT", b =>
                {
                    b.Property<int>("TokenId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Creator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsListed")
                        .HasColumnType("bit");

                    b.Property<string>("Owner")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("TokenAddress")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("TokenURI")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("TokenId");

                    b.HasIndex("CategoryId");

                    b.ToTable("NFT");
                });

            modelBuilder.Entity("NFTMarketPlace_Backend.Models.Account", b =>
                {
                    b.Property<string>("AccountAddress")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AccountEmail")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BannerImage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("AccountAddress");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("NFTMarketPlace_Backend.Models.Category", b =>
                {
                    b.Property<int>("CategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryId"));

                    b.Property<string>("AccountAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CategoryImage")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CategoryId");

                    b.HasIndex("AccountAddress");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("NFTMarketPlace_Backend.Models.NFTTransaction", b =>
                {
                    b.Property<int>("NftTransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NftTransactionId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Event")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NftAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Price")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("tokenId")
                        .HasColumnType("int");

                    b.HasKey("NftTransactionId");

                    b.ToTable("NFTTransaction");
                });

            modelBuilder.Entity("NFT", b =>
                {
                    b.HasOne("NFTMarketPlace_Backend.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("NFTMarketPlace_Backend.Models.Category", b =>
                {
                    b.HasOne("NFTMarketPlace_Backend.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountAddress")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });
#pragma warning restore 612, 618
        }
    }
}
