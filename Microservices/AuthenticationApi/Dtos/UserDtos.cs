using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Dtos
{
    public record RegisterDto(
        [Required] string FullName,
        [EmailAddress] string Email,
        [Phone] string PhoneNumber,
        [Required][Length(6,12)] string Password,
        [Required] Guid RoleId );
    public record LoginDto([EmailAddress] string Email, [Required] string Password);
    public record GetUserDto(Guid Id, string FullName, string Email, string PhoneNumber, Guid RoleId);
    public record UpdateUserDto(
        [Required] Guid Id, 
        [Required] string FullName,  
        [Required] string PhoneNumber, 
        Guid RoleId);
}
