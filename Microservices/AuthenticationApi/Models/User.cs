using Common.Interfaces;

namespace AuthenticationApi.Models
{
    public class User : IBaseEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password{ get; set; }
        public string ResetToken { get; set; } = string.Empty;
        public DateTime ResetTokenExpiresAt { get; set; } = DateTime.UtcNow; 
        public Role Role { get; set; } 
    }
}
