﻿using AuthenticationApi.Dtos;
using AuthenticationApi.Enums; 
using AuthenticationApi.Interfaces;
using AuthenticationApi.Models;
using AuthenticationApi.Utilities;
using Common.Dtos;
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

        public async Task<ServiceResult<IEnumerable<User>>> GetAllUsersAsync() 
        { 
            var result = await _getAllRepository.GetAll();

            return result.Any() ?
                ServiceResult<IEnumerable<User>>.Success("Users exists!", result) :
                ServiceResult<IEnumerable<User>>.Failure("No user exists inside the server");
        }

        public async Task<ServiceResult<User>> GetUserByIdAsync(Guid id)
        {
            var result = await _getRepository.Get(id);

            return result is not null ?
                ServiceResult<User>.Success("User was found!", result) :
                ServiceResult<User>.Failure($"No user exists inside the server with this id: {id}");
        }
       
        public async Task<ServiceResult<User>> LoginAsync(LoginDto dto)
        {
            var exists = await _getRepository.Get(x=>x.Email == dto.Email);

            if(exists is null) 
                return ServiceResult<User>.Failure(
                    $"No user exists inside the server with this email: {dto.Email}");

            var hashResult = UserSecurityService.VerifyPassword(exists.HashedPassword, dto.Password);

            if (!hashResult)
                return ServiceResult<User>.Failure("Incorrect Password");

            return ServiceResult<User>.Success("Logged in successfully", exists);
        }

        public async Task<ServiceResult<bool>> RegisterAsync(RegisterDto dto)
        {
            var exists = await _getRepository.Get(x => x.Email == dto.Email || x.PhoneNumber == dto.PhoneNumber);

            if (exists is not null) 
                return ServiceResult<bool>.Failure(
                    "User already exists with same email or phone please enter a unique valued");

            var hashedPassword = UserSecurityService.HashPassword(dto.Password);

            var role = CheckRole(dto.Role);

            if (role == Role.NONE) 
                return ServiceResult<bool>.Failure("Role sent was not found");

            var newUser = User.Factory(
                dto.FullName, 
                dto.Email,
                dto.PhoneNumber, 
                hashedPassword, 
                role, 
                SSNHashing.ComputeSHA256(dto.SSN.ToString()));

            var result = await _createRepository.CreateAsync(newUser);

            return result ?
                ServiceResult<bool>.Success("New account was created successfully") :
                ServiceResult<bool>.Failure("Failed to create new user"); 
        }

        public async Task<ServiceResult<string>> ForgetPasswordAsync(string email)
        {
            var user = await _getRepository.Get(x => x.Email == email);

            var genertatedToken = UserSecurityService.GenerateResetToken(16);

            user.ResetUserPasswordToken(genertatedToken, DateTime.UtcNow.AddHours(1)); 

            var result = await _updateRepository.UpdateAsync(user);

            return result ?
                ServiceResult<string>.Success("Reset token was set successfully", genertatedToken) :
                ServiceResult<string>.Failure("Failed to create a reset password token");
        }

        public async Task<ServiceResult<bool>> ResetPasswordAsync(ResetPasswordDto dto, string token)
        {
            var user = await _getRepository.Get(x => x.ResetToken == dto.ResetToken);

            if (user.ResetToken != token || user.ResetTokenExpiresAt < DateTime.UtcNow) 
                return ServiceResult<bool>.Failure("Incorrect or expired reset token");

            user.ResetUserHashedPassword(dto.NewPassword);

            var result = await _updateRepository.UpdateAsync(user);

            return result ?
                ServiceResult<bool>.Success("Password was updated successfully") :
                ServiceResult<bool>.Failure("Failed to reset password");
        }

        public async Task<ServiceResult<bool>> UpdateUserAsync(UpdateUserDto dto)
        {
            var exists = await _getRepository.Get(x => x.Id == dto.Id);

            var failMessage = "Failed to update user info";

            if (exists is null)
                return ServiceResult<bool>.Failure(failMessage);

            exists.UpdateUser(dto.FullName, dto.PhoneNumber);

            var result = await _updateRepository.UpdateAsync(exists);  

            return result ?
               ServiceResult<bool>.Success("User was updated successfully") :
               ServiceResult<bool>.Failure(failMessage);
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
