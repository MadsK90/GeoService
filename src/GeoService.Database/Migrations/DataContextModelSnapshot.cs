﻿// <auto-generated />
using System;
using GeoService.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeoService.Database.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.4");

            modelBuilder.Entity("GeoService.Database.Models.Cabinet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Cabinets");
                });

            modelBuilder.Entity("GeoService.Database.Models.Fibre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Air")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Size")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Fibres");
                });

            modelBuilder.Entity("GeoService.Database.Models.Manhole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Manholes");
                });

            modelBuilder.Entity("GeoService.Database.Models.PointDouble", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("FibreId")
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<Guid?>("PolygonId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("RouteId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FibreId");

                    b.HasIndex("PolygonId");

                    b.HasIndex("RouteId");

                    b.ToTable("PointDouble");
                });

            modelBuilder.Entity("GeoService.Database.Models.Polygon", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Polygons");
                });

            modelBuilder.Entity("GeoService.Database.Models.Route", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("GeoService.Database.Models.Splitter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Splitters");
                });

            modelBuilder.Entity("GeoService.Database.Models.PointDouble", b =>
                {
                    b.HasOne("GeoService.Database.Models.Fibre", null)
                        .WithMany("Points")
                        .HasForeignKey("FibreId");

                    b.HasOne("GeoService.Database.Models.Polygon", null)
                        .WithMany("Points")
                        .HasForeignKey("PolygonId");

                    b.HasOne("GeoService.Database.Models.Route", null)
                        .WithMany("Points")
                        .HasForeignKey("RouteId");
                });

            modelBuilder.Entity("GeoService.Database.Models.Fibre", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("GeoService.Database.Models.Polygon", b =>
                {
                    b.Navigation("Points");
                });

            modelBuilder.Entity("GeoService.Database.Models.Route", b =>
                {
                    b.Navigation("Points");
                });
#pragma warning restore 612, 618
        }
    }
}
