﻿// <auto-generated />
using System;
using Lab4.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Lab4.Migrations
{
    [DbContext(typeof(MarketDbContext))]
    partial class MarketDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Lab4.Models.Advertisement", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Advertisement", (string)null);
                });

            modelBuilder.Entity("Lab4.Models.AdvertisementBrokerage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AdvertisementId")
                        .HasColumnType("int");

                    b.Property<string>("BrokerageId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("AdvertisementId");

                    b.HasIndex("BrokerageId");

                    b.ToTable("AdvertisementBrokerage", (string)null);
                });

            modelBuilder.Entity("Lab4.Models.Brokerage", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("ClientID")
                        .HasColumnType("int");

                    b.Property<decimal>("Fee")
                        .HasColumnType("money");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("ClientID");

                    b.ToTable("Brokerage", (string)null);
                });

            modelBuilder.Entity("Lab4.Models.Client", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)")
                        .HasColumnName("FirstName");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Client", (string)null);
                });

            modelBuilder.Entity("Lab4.Models.Subscription", b =>
                {
                    b.Property<int>("ClientId")
                        .HasColumnType("int");

                    b.Property<string>("BrokerageId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ID")
                        .HasColumnType("int");

                    b.HasKey("ClientId", "BrokerageId");

                    b.HasIndex("BrokerageId");

                    b.ToTable("Subscriptiom", (string)null);
                });

            modelBuilder.Entity("Lab4.Models.AdvertisementBrokerage", b =>
                {
                    b.HasOne("Lab4.Models.Advertisement", "Advertisement")
                        .WithMany()
                        .HasForeignKey("AdvertisementId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab4.Models.Brokerage", "Brokerage")
                        .WithMany()
                        .HasForeignKey("BrokerageId");

                    b.Navigation("Advertisement");

                    b.Navigation("Brokerage");
                });

            modelBuilder.Entity("Lab4.Models.Brokerage", b =>
                {
                    b.HasOne("Lab4.Models.Client", null)
                        .WithMany("Brokerages")
                        .HasForeignKey("ClientID");
                });

            modelBuilder.Entity("Lab4.Models.Subscription", b =>
                {
                    b.HasOne("Lab4.Models.Brokerage", "Brokerages")
                        .WithMany("Subscriptions")
                        .HasForeignKey("BrokerageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Lab4.Models.Client", "Client")
                        .WithMany("Subscriptions")
                        .HasForeignKey("ClientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Brokerages");

                    b.Navigation("Client");
                });

            modelBuilder.Entity("Lab4.Models.Brokerage", b =>
                {
                    b.Navigation("Subscriptions");
                });

            modelBuilder.Entity("Lab4.Models.Client", b =>
                {
                    b.Navigation("Brokerages");

                    b.Navigation("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
