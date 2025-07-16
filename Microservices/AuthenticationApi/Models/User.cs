using AuthenticationApi.Enums; 

namespace AuthenticationApi.Models
{
    public class User : BaseEntity
    {
        private User() { }
        private User(
            string fullName,
            string email,
            string phoneNumber, 
            string hashedPassword, 
            Role role,
            string hashedSSN
        )
        {
            FullName = fullName;
            Email = email;
            PhoneNumber = phoneNumber;
            HashedPassword = hashedPassword;
            Role = role;
            ResetToken = string.Empty;
            ResetTokenExpiresAt = DateTime.UtcNow;
            CreatedAt = DateTime.UtcNow;
            HashedSSN = hashedSSN;
        }

        public static User Factory(
            string fullName,
            string email,
            string phoneNumber, 
            string hashedPassword, 
            Role role,
            string hashedSSN
        )
        => new User(
             fullName,
             email,
             phoneNumber,
             hashedPassword,
             role,
             hashedSSN
        );

        public void UpdateUser(string fullName, string phoneNumber)
        {
            FullName = fullName;
            PhoneNumber = phoneNumber;
            MarkAsUpdated();
        }
        public void ResetUserPasswordToken(string resetToken, DateTime resetTokenExpiresAt)
        {
            ResetToken = resetToken;
            ResetTokenExpiresAt = resetTokenExpiresAt;
        }

        public void ResetUserHashedPassword(string hashedPassword)
        {
            HashedPassword = hashedPassword;  
        }

        public string FullName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string HashedPassword{ get; private set; }
        public string HashedSSN { get; private set; }
        public string ResetToken { get; private set; } 
        public DateTime ResetTokenExpiresAt { get; private set; }
        public Role Role { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
