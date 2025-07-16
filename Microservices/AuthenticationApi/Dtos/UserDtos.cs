using AuthenticationApi.CustomeValidators;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Dtos
{
    public record RegisterDto(
        [Required] string FullName,
        [EmailAddress] string Email,
        [Phone] string PhoneNumber,
        [Required][Length(6,12)] string Password,
        [Required] string Role,
        [Required][SSNValidation] long SSN);

    public record LoginDto([EmailAddress] string Email, [Required] string Password);
    
    public record ResetPasswordDto([Required] string ResetToken, [Required] string NewPassword);
    
    public record GetUserDto(Guid Id, string FullName, string Email, string PhoneNumber, string Role);
    
    public record UpdateUserDto(
        [Required] Guid Id, 
        [Required] string FullName,  
        [Required] string PhoneNumber
    );
}
