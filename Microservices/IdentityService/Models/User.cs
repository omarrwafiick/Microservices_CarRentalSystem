namespace IdentityServiceApi.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Role Role { get; set; } 
    }
    public enum Role
    {
        Customer,
        Admin
    }
}
