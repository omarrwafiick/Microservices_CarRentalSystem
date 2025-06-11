using AuthenticationApi.Dtos;
using AuthenticationApi.Extensions;
using AuthenticationApi.Interfaces;
using AuthenticationApi.Models;
using AuthenticationApi.Utilities;
using Common.Interfaces;

namespace AuthenticationApi.Services
{
    public class UserService : IUserService
    {
        private readonly IGetAllRepository<User> _getAllRepository;
        private readonly IGetRepository<User> _getRepository;
        private readonly ICreateRepository<User> _createRepository;
        private readonly IUpdateRepository<User> _updateRepository;
        public UserService(
            IGetAllRepository<User> getAllRepository, 
            IGetRepository<User> getRepository, 
            ICreateRepository<User> createRepository, 
            IUpdateRepository<User> updateRepository)
        {
            _getAllRepository = getAllRepository;
            _getRepository = getRepository;
            _createRepository = createRepository;
            _updateRepository = updateRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _getAllRepository.GetAll();

        public async Task<User> GetUserByIdAsync(Guid id) => await _getRepository.Get(id);

        public async Task<User> LoginAsync(LoginDto dto)
        {
            var exists = await _getRepository.Get(x=>x.Email == dto.Email);
            if(exists is null) return null;
            var hashResult = UserSecurityService.VerifyPassword(exists.Password, dto.Password);
            if (!hashResult) return null;
            return exists;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var exists = await _getRepository.Get(x => x.Email == dto.Email);
            if (exists is not null) return false;
            var newUser = dto.RegisterMapFromDtoToDomain();
            var result = await _createRepository.CreateAsync(newUser);
            return result;
        }

        public async Task<bool> ForgetPasswordAsync(string email)
        {
            var user = await _getRepository.Get(x => x.Email == email);

            user.ResetToken = UserSecurityService.GenerateResetToken(16);
            user.ResetTokenExpiresAt = DateTime.UtcNow.AddHours(1);

            return await _updateRepository.UpdateAsync(user); 
        }

        public async Task<bool> ResetPasswordAsync(LoginDto dto, string token)
        {
            var user = await _getRepository.Get(x => x.Email == dto.Email);

            if (user.ResetToken != token || user.ResetTokenExpiresAt < DateTime.UtcNow) 
                return false;

            user.Password = dto.Password;
            return await _updateRepository.UpdateAsync(user); ;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var exists = await _getRepository.Get(x => x.Id == dto.Id);
            if (exists is null) return false;
            var updateUser = exists.UpdateMapFromDtoToDomain(dto);
            var result = await _updateRepository.UpdateAsync(updateUser);
            return result;
        }
    }
}
