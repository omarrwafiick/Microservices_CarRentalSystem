using System.Security.Cryptography; 

namespace AuthenticationApi.Utilities
{
    public static class UserSecurityService
    {
        private const int SaltSize = 16;  
        private const int HashSize = 32;  
        private const int Iterations = 100_000;
        public static string HashPassword(string password) => HashPasswordSec(password);

        public static bool VerifyPassword(string hashedPassword, string inputPassword) => VerifyPasswordSec(hashedPassword, inputPassword);

        private static string HashPasswordSec(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            byte[] salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        private static bool VerifyPasswordSec(string storedHash, string inputPassword)
        {
            byte[] hashBytes;

            try
            {
                hashBytes = Convert.FromBase64String(storedHash);
            }
            catch
            {
                return false; 
            }

            if (hashBytes.Length != SaltSize + HashSize)
                return false;

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, Iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            for (int i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }

            return true;
        }

        public static string GenerateResetToken(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var bytes = new byte[length];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            return new string(bytes.Select(b => chars[b % chars.Length]).ToArray());
        }
    }

}
