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
                domain.Password,
                domain.RoleId
                );
        }

        public static User UpdateMapFromDtoToDomain(this User domain, UpdateUserDto dto)
        {
            domain.Email = dto.Email;
            domain.FullName = dto.FullName;
            domain.PhoneNumber = dto.PhoneNumber;
            domain.RoleId = dto.RoleId;
            return domain;
        }
    }
}
