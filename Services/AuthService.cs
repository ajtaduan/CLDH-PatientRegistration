using Microsoft.AspNetCore.Identity;
using CLDH.PatientRegistration.Models;

namespace CLDH.PatientRegistration.Services
{
    // Using ASP.NET built in hasher instead of writing my own hashing logic
    public class AuthService
    {
        private readonly PasswordHasher<User> _hasher = new();

        // Turns a plain password into a hash before saving to DB
        public string HashPassword(User user, string plainPassword)
        {
            return _hasher.HashPassword(user, plainPassword);
        }

        // Checks if a login attemp matches the stored hash
        public bool VerifyPassword(User user, string hashedPassword, string providedPassword)
        {
            var result = _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}