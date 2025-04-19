using Microsoft.EntityFrameworkCore;
using VehicleServiceApi.Models;

namespace VehicleServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
          
        public DbSet<Vehicle> Vehicles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vehicle>().HasData(
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "ABC1234",
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = 2020,
                    Type = "Sedan",
                    DailyRate = 50.00m,
                    IsAvailable = true,
                    Location = "New York"
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "XYZ5678",
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2021,
                    Type = "Sedan",
                    DailyRate = 55.00m,
                    IsAvailable = true,
                    Location = "Los Angeles"
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "LMN9876",
                    Make = "Ford",
                    Model = "F-150",
                    Year = 2022,
                    Type = "Truck",
                    DailyRate = 80.00m,
                    IsAvailable = true,
                    Location = "Chicago"
                }
            );
        }
    }
}
