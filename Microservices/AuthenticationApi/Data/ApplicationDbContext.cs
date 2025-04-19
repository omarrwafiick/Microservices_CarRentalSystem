using AuthenticationApi.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
         
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                    new Role
                    {
                        Id = Guid.NewGuid(),
                        Name = "Admin"
                    },
                    new Role { 
                        Id = Guid.NewGuid(), 
                        Name = "User" 
                    }
            );
        }
    }
}
