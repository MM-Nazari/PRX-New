using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace PRX.Utils
{
    public class Utils
    {
        public string HashPassword(string password)
        {
            // Generate a random salt
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password using PBKDF2
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Combine the salt and hashed password and return as a single string
            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and hashed password from the stored hash
            string[] hashParts = hashedPassword.Split(':');

            // Ensure that the hashParts array contains at least two elements
            if (hashParts.Length < 2)
            {
                // Handle the case where the hashed password is not properly formatted
                return false;
            }

            byte[] salt = Convert.FromBase64String(hashParts[0]);
            string storedHash = hashParts[1];

            // Compute the hash of the provided password with the same salt
            string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));

            // Compare the computed hash with the stored hash
            return storedHash == computedHash;
        }


        //public string HashPassword(string password)
        //{
        //    // Generate a random salt
        //    byte[] salt = new byte[16];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(salt);
        //    }

        //    // Hash the password using PBKDF2
        //    string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 10000,
        //        numBytesRequested: 32));

        //    // Return the hashed password only (without combining with salt)
        //    return hashedPassword;
        //}

        //public bool VerifyPassword(string password, string hashedPassword)
        //{
        //    // Extract the salt and hashed password from the stored hash
        //    string[] hashParts = hashedPassword.Split(':');
        //    byte[] salt = Convert.FromBase64String(hashParts[0]);
        //    string storedHash = hashParts[1];

        //    // Compute the hash of the provided password with the same salt
        //    string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //        password: password,
        //        salt: salt,
        //        prf: KeyDerivationPrf.HMACSHA256,
        //        iterationCount: 10000,
        //        numBytesRequested: 32));

        //    // Compare the computed hash with the stored hash
        //    return storedHash == computedHash;
        //}

    }
}
