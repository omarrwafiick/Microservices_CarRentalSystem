using Microsoft.EntityFrameworkCore;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
          
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<VehicleImages> VehicleImages { get; set; }
        public DbSet<MaintenanceCenter> MaintenanceCenters { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }  
    }
}
