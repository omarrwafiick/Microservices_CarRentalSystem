using AuthenticationApi.Dtos;
using AuthenticationApi.Models;

namespace AuthenticationApi.Interfaces
{
    public interface IUserService
    {
        Task<User> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserAsync(UpdateUserDto dto);
        Task<bool> ForgetPasswordAsync(string email);
        Task<bool> ResetPasswordAsync(LoginDto dto, string token);
    }
}
