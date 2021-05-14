using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Security.Cryptography;

namespace UserManagement.Models
{
    public class Hasher
    {
        public string GenerateSalt(int num_bytes)
        {
            byte[] salt = new byte[num_bytes];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public string HashPassword(string password, string salt, int iterations, int bytes)
        {
            return Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password
                    , salt: Convert.FromBase64String(salt)
                    , prf: KeyDerivationPrf.HMACSHA1
                    , iterationCount: iterations
                    , numBytesRequested: bytes
                )
            );
        }
    }
}
