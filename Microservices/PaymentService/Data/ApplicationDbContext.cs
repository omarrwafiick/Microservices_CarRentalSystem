 
using Microsoft.EntityFrameworkCore;
using PaymentService.Models;
using PaymentServiceApi.Models;

namespace PaymentService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<PaymentStatus> PaymentStatus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PaymentMethod>().HasData(
                    new PaymentMethod
                    {
                        Id = Guid.NewGuid(),
                        Name = "CreditCard"
                    },
                    new PaymentMethod
                    {
                        Id = Guid.NewGuid(),
                        Name = "DebitCard"
                    },
                    new PaymentMethod
                    {
                        Id = Guid.NewGuid(),
                        Name = "PayPal"
                    },
                    new PaymentMethod
                    {
                        Id = Guid.NewGuid(),
                        Name = "Wallet"
                    }
            );

            modelBuilder.Entity<PaymentStatus>().HasData(
                   new PaymentStatus
                   {
                       Id = Guid.NewGuid(),
                       Status = "Pending"
                   },
                   new PaymentStatus
                   {
                       Id = Guid.NewGuid(),
                       Status = "Completed"
                   },
                   new PaymentStatus
                   {
                       Id = Guid.NewGuid(),
                       Status = "Failed"
                   }
           );
        }
    }
}
