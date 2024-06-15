﻿// <auto-generated />
using System;
using DAL.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DAL.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.4.24267.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DAL.Entities.Aisle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Aisles");
                });

            modelBuilder.Entity("DAL.Entities.Bin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CellId")
                        .HasColumnType("int");

                    b.Property<int?>("InventoryItemId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("ShelfId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CellId");

                    b.HasIndex("InventoryItemId")
                        .IsUnique()
                        .HasFilter("[InventoryItemId] IS NOT NULL");

                    b.HasIndex("ShelfId");

                    b.ToTable("Bins");
                });

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL.Entities.Cell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AisleId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int?>("RackId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AisleId");

                    b.HasIndex("RackId");

                    b.ToTable("Cells");
                });

            modelBuilder.Entity("DAL.Entities.InventoryItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("BinId")
                        .HasColumnType("int");

                    b.Property<string>("Container")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ItemId")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocationDest")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.ToTable("InventoryItems");
                });

            modelBuilder.Entity("DAL.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Company")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TurnoverRate")
                        .HasColumnType("int");

                    b.Property<int?>("WarehouseId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("DAL.Entities.Rack", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AisleId")
                        .HasColumnType("int");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("WarehouseId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AisleId");

                    b.HasIndex("WarehouseId");

                    b.ToTable("Racks");
                });

            modelBuilder.Entity("DAL.Entities.Shelf", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("RackId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RackId");

                    b.ToTable("Shelves");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Login = "admin",
                            Password = "12345",
                            Role = "admin"
                        });
                });

            modelBuilder.Entity("DAL.Entities.Warehouse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MaximumWeightOnUpperShelves")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("DAL.Entities.Aisle", b =>
                {
                    b.HasOne("DAL.Entities.Warehouse", "Warehouse")
                        .WithMany("Aisles")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DAL.Entities.Bin", b =>
                {
                    b.HasOne("DAL.Entities.Cell", "Cell")
                        .WithMany("Bins")
                        .HasForeignKey("CellId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DAL.Entities.InventoryItem", "InventoryItem")
                        .WithOne("Bin")
                        .HasForeignKey("DAL.Entities.Bin", "InventoryItemId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DAL.Entities.Shelf", "Shelf")
                        .WithMany("Bins")
                        .HasForeignKey("ShelfId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Cell");

                    b.Navigation("InventoryItem");

                    b.Navigation("Shelf");
                });

            modelBuilder.Entity("DAL.Entities.Cell", b =>
                {
                    b.HasOne("DAL.Entities.Aisle", null)
                        .WithMany("Cells")
                        .HasForeignKey("AisleId");

                    b.HasOne("DAL.Entities.Rack", null)
                        .WithMany("Cells")
                        .HasForeignKey("RackId");
                });

            modelBuilder.Entity("DAL.Entities.InventoryItem", b =>
                {
                    b.HasOne("DAL.Entities.Item", "Item")
                        .WithMany()
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("DAL.Entities.Item", b =>
                {
                    b.HasOne("DAL.Entities.Category", "Category")
                        .WithMany("Items")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DAL.Entities.Warehouse", null)
                        .WithMany("Items")
                        .HasForeignKey("WarehouseId");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DAL.Entities.Rack", b =>
                {
                    b.HasOne("DAL.Entities.Aisle", "Aisle")
                        .WithMany("Racks")
                        .HasForeignKey("AisleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("DAL.Entities.Warehouse", "Warehouse")
                        .WithMany("Racks")
                        .HasForeignKey("WarehouseId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Aisle");

                    b.Navigation("Warehouse");
                });

            modelBuilder.Entity("DAL.Entities.Shelf", b =>
                {
                    b.HasOne("DAL.Entities.Rack", "Rack")
                        .WithMany("Shelves")
                        .HasForeignKey("RackId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Rack");
                });

            modelBuilder.Entity("DAL.Entities.Warehouse", b =>
                {
                    b.HasOne("DAL.Entities.User", "User")
                        .WithMany("Warehouses")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DAL.Entities.Aisle", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Racks");
                });

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("DAL.Entities.Cell", b =>
                {
                    b.Navigation("Bins");
                });

            modelBuilder.Entity("DAL.Entities.InventoryItem", b =>
                {
                    b.Navigation("Bin")
                        .IsRequired();
                });

            modelBuilder.Entity("DAL.Entities.Rack", b =>
                {
                    b.Navigation("Cells");

                    b.Navigation("Shelves");
                });

            modelBuilder.Entity("DAL.Entities.Shelf", b =>
                {
                    b.Navigation("Bins");
                });

            modelBuilder.Entity("DAL.Entities.User", b =>
                {
                    b.Navigation("Warehouses");
                });

            modelBuilder.Entity("DAL.Entities.Warehouse", b =>
                {
                    b.Navigation("Aisles");

                    b.Navigation("Items");

                    b.Navigation("Racks");
                });
#pragma warning restore 612, 618
        }
    }
}
