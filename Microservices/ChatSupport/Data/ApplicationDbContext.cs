
using ChatSupportApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatSupportApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Chat> Chat { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}
