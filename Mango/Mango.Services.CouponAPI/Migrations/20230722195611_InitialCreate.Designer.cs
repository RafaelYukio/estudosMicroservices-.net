﻿// <auto-generated />
using System;
using Mango.Services.CouponAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Mango.Services.CouponAPI.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20230722195611_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mango.Services.CouponAPI.Models.Coupon", b =>
                {
                    b.Property<Guid>("CouponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("DiscountAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MinAmount")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CouponId");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            CouponId = new Guid("d38af154-f99a-4917-bece-6d159a563a99"),
                            CouponCode = "10OFF",
                            DiscountAmount = 10m,
                            MinAmount = 20m
                        },
                        new
                        {
                            CouponId = new Guid("a8bfb8aa-ea80-408e-ab10-ad53e3480e63"),
                            CouponCode = "20OFF",
                            DiscountAmount = 20m,
                            MinAmount = 40m
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
