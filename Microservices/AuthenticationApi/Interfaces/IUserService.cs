using AuthenticationApi.Dtos;
using AuthenticationApi.Models;
using Common.Dtos;

namespace AuthenticationApi.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<User>> LoginAsync(LoginDto dto);
        Task<ServiceResult<bool>> RegisterAsync(RegisterDto dto);
        Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync();
        Task<ServiceResult<User>> GetUserByIdAsync(int id);
        Task<ServiceResult<bool>> UpdateUserAsync(UpdateUserDto dto);
        Task<ServiceResult<string>> ForgetPasswordAsync(string email);
        Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordDto dto, string token);
    }
}
