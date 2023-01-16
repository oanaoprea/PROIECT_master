using Microsoft.EntityFrameworkCore;
using PROIECT.Models;
using System.Security.Policy;

namespace PROIECT.Data
{
    public class PlantsContext:DbContext
    {
        public PlantsContext(DbContextOptions<PlantsContext> options) : base(options) 
        { 

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<AvailablePlant> AvailablePlants { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            modelBuilder.Entity<Customer>().ToTable("Customer"); 
            modelBuilder.Entity<Order>().ToTable("Order"); 
            modelBuilder.Entity<Plant>().ToTable("Plant");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Shop>().ToTable("Shop"); 
            modelBuilder.Entity<AvailablePlant>().ToTable("AvailablePlant");

            modelBuilder.Entity<AvailablePlant>().HasKey(c => new { c.PlantID, c.ShopID });
        }
    }
}
