using Microsoft.EntityFrameworkCore;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
          
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<VehicleModel> Models { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<VehicleImages> VehicleImages { get; set; }
        public DbSet<MaintenanceCenter> MaintenanceCenters { get; set; }
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Seeding
            #region Variables
                var make1 = Make.Factory("Toyota");
                var make2 = Make.Factory("Ford");
                var make3 = Make.Factory("Honda");
            #endregion

            modelBuilder.Entity<Make>().HasData(
               make1,
               make2,
               make3
            );

            #region Variables 
                var model1 = VehicleModel.Factory("Corolla", make1.Id);
                var model2 = VehicleModel.Factory("Camry", make1.Id);
                var model3 = VehicleModel.Factory("F-150", make2.Id);
                var model4 = VehicleModel.Factory("Civic", make3.Id);
                var model5 = VehicleModel.Factory("Accord", make3.Id);
            #endregion

            modelBuilder.Entity<VehicleModel>().HasData(
                model1,
                model2,
                model3,
                model4,
                model5
            );
            #endregion
        }
    }
}
