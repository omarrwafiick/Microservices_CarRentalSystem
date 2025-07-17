 
using Microsoft.EntityFrameworkCore; 
using PaymentService.Models; 

namespace PaymentService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<PaymentRecord> PaymentsRecords { get; set; } 
    }
}
