using AuthenticationApi.Dtos;
using AuthenticationApi.Enums; 
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

            if(exists is null) 
                return null;

            var hashResult = UserSecurityService.VerifyPassword(exists.HashedPassword, dto.Password);

            if (!hashResult)
                return null;

            return exists;
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            var exists = await _getRepository.Get(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber);

            if (exists is not null) 
                return false;

            var hashedPassword = UserSecurityService.HashPassword(dto.Password);

            var role = CheckRole(dto.Role);

            if (role == Role.NONE) 
                return false;

            var newUser = User.Factory(
                dto.FullName, 
                dto.Email,
                dto.PhoneNumber, 
                hashedPassword, 
                role, 
                SSNHashing.ComputeSHA256(dto.SSN.ToString()));

            var result = await _createRepository.CreateAsync(newUser);

            return result;
        }

        public async Task<string> ForgetPasswordAsync(string email)
        {
            var user = await _getRepository.Get(x => x.Email == email);

            var genertatedToken = UserSecurityService.GenerateResetToken(16);

            user.ResetUserPasswordToken(genertatedToken, DateTime.UtcNow.AddHours(1)); 

            await _updateRepository.UpdateAsync(user);

            return genertatedToken;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto, string token)
        {
            var user = await _getRepository.Get(x => x.ResetToken == dto.ResetToken);

            if (user.ResetToken != token || user.ResetTokenExpiresAt < DateTime.UtcNow) 
                return false;

            user.ResetUserHashedPassword(dto.NewPassword);
            return await _updateRepository.UpdateAsync(user); ;
        }

        public async Task<bool> UpdateUserAsync(UpdateUserDto dto)
        {
            var exists = await _getRepository.Get(x => x.Id == dto.Id);

            if (exists is null) 
                return false;

            exists.UpdateUser(dto.FullName, dto.PhoneNumber);

            var result = await _updateRepository.UpdateAsync(exists);

            return result;
        }

        private Role CheckRole(string UserRole)
        {
            var roles = Enum.GetValues<Role>().OfType<string>().ToList();
            Role role = Role.NONE;

            for (int r = 0; r < roles.Count(); r++)
            {
                if (roles[r].ToLower() == UserRole.ToLower())
                {
                    role = Enum.Parse<Role>(roles[r]);
                }
            }

            return role;
        }

         
    }
}
