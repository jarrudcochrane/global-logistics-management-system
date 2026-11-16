using GLMS.Models;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Data
{
    // This class connects our C# models to the actual database
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }           
        public DbSet<Contract> Contracts { get; set; }       
        public DbSet<ServiceRequest> ServiceRequests { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Make sure CostUSD stores up to 18 digits total, 2 after decimal point
            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.CostUSD)
                .HasPrecision(18, 2);

            // Same precision for CostZAR
            modelBuilder.Entity<ServiceRequest>()
                .Property(s => s.CostZAR)
                .HasPrecision(18, 2);
        }
    }
}