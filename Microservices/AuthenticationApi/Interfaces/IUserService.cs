using AuthenticationApi.Dtos;
using AuthenticationApi.Models;

namespace AuthenticationApi.Interfaces
{
    public interface IUserService
    {
        Task<bool> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<bool> UpdateUserAsync(UpdateUserDto dto);
    }
}
