using AuthenticationApi.Dtos;
using AuthenticationApi.Models;
using AuthenticationApi.Utilities;

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
                domain.Password,
                domain.RoleId
                );
        }

        public static User UpdateMapFromDtoToDomain(this User domain, UpdateUserDto dto)
        { 
            domain.FullName = dto.FullName;
            domain.PhoneNumber = dto.PhoneNumber;
            domain.RoleId = dto.RoleId;
            return domain;
        }

        public static User RegisterMapFromDtoToDomain(this RegisterDto dto)
        {
            return new User
            {
               FullName = dto.FullName,
               Email = dto.Email,
               PhoneNumber = dto.PhoneNumber,
               Password = UserSecurityService.HashPassword(dto.Password),
               RoleId = dto.RoleId
            };
        }
    }
}
