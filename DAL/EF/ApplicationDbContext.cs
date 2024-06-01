using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.EF
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Cell> Cells { get; set; }
        public DbSet<Aisle> Aisles { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Bin> Bins { get; set; }

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Database configuration code
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\Local;Database=GreeMed");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships and constraints
            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Item)
                .WithMany()
                .HasForeignKey(ii => ii.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<InventoryItem>()
                .HasOne(ii => ii.Bin)
                .WithOne(b => b.InventoryItem)
                .HasForeignKey<InventoryItem>(ii => ii.BinId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Item>()
                .HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Aisle>()
                .HasOne(a => a.Warehouse)
                .WithMany(w => w.Aisles)
                .HasForeignKey(a => a.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rack>()
                .HasOne(r => r.Warehouse)
                .WithMany(w => w.Racks)
                .HasForeignKey(r => r.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Rack>()
                .HasOne(r => r.Aisle)
                .WithMany(a => a.Racks)
                .HasForeignKey(r => r.AisleId)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Shelf>()
                .HasOne(s => s.Rack)
                .WithMany(r => r.Shelves)
                .HasForeignKey(s => s.RackId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bin>()
                .HasOne(b => b.Shelf)
                .WithMany(s => s.Bins)
                .HasForeignKey(b => b.ShelfId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bin>()
                .HasOne(b => b.InventoryItem)
                .WithOne(ii => ii.Bin)
                .HasForeignKey<Bin>(b => b.InventoryItemId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Bin>()
                .HasOne(b => b.Cell)
                .WithMany(c => c.Bins)
                .HasForeignKey(b => b.CellId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
