
using BookingServiceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BookingStatus>().HasData(
                    new BookingStatus
                    {
                        Id = Guid.NewGuid(),
                        Status = "Pending"
                    },
                    new BookingStatus
                    {
                        Id = Guid.NewGuid(),
                        Status = "Confirmed"
                    },
                    new BookingStatus
                    {
                        Id = Guid.NewGuid(),
                        Status = "Cancelled"
                    },
                    new BookingStatus
                    {
                        Id = Guid.NewGuid(),
                        Status = "Completed"
                    }
            );
        }
    }
}
