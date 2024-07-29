using Audit.EntityFramework;
using Microsoft.EntityFrameworkCore;
using StockSmart.Domain.Entities;

namespace StockSmart.Infrastructure
{

    public class AppDbContext : AuditDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Status>().HasData(
                new { StatusId = 1, Name = "Active" },
                new { StatusId = 2, Name = "Inactive" });
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Status> Status { get; set; }
    }
}
