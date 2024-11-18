using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace JavaHateBE.Util
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords.
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Hashes the specified password.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <returns>The hashed password.</returns>
        public static string HashPassword(string password)
        {
            Console.WriteLine("Hashing password: " + password);

            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return $"{Convert.ToBase64String(salt)}.{hashed}";
        }

        /// <summary>
        /// Verifies the specified password against a hashed password.
        /// </summary>
        /// <param name="password">The password to verify.</param>
        /// <param name="hashedPassword">The hashed password to compare against.</param>
        /// <returns>True if the password is verified, otherwise false.</returns>
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            Console.WriteLine("Verifying password: " + password);

            var parts = hashedPassword.Split('.');
            if (parts.Length != 2)
            {
                return false;
            }

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed == storedHash;
        }
    }
}
