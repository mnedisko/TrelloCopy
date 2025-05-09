using Microsoft.AspNetCore.Identity;

namespace TrelloCopy.Services
{
    public class PasswordService
    {
        private readonly PasswordHasher<string> _passwordHasher = new PasswordHasher<string>();
        public string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }
        public bool VerifyPassword(string hasshedPassword, string providedPassword)
        {
            return _passwordHasher.VerifyHashedPassword(null, hasshedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
