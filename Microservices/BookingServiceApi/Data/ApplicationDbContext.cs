
using BookingServiceApi.Models;
using Microsoft.EntityFrameworkCore;

namespace BookingServiceApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; } 
       
    }
}
