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
        public Guid RoleId { get; set; }
        public Role Role { get; set; } 
    }
}
