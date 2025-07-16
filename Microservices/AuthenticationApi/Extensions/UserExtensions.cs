using AuthenticationApi.Dtos; 
using AuthenticationApi.Models; 

namespace AuthenticationApi.Extensions
{
    public static class UserExtensions
    {
        public static GetUserDto MapFromDomainToDto(this User domain)
        {
            return new GetUserDto(
                domain.Id,
                domain.FullName,
                domain.Email,
                domain.PhoneNumber,
                domain.Role.ToString()
            );
        }
    }
}
