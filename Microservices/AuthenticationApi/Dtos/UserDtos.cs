namespace AuthenticationApi.Dtos
{
    public record RegisterDto( Guid Id, string FullName, string Email, string PhoneNumber, string Password, Guid RoleId );
    public record LoginDto(string Email, string Password);
    public record GetUserDto(Guid Id, string FullName, string Email, string PhoneNumber, Guid RoleId);
    public record UpdateUserDto(string FullName, string Email, string PhoneNumber, Guid RoleId);
}
