using Artemis.Backend.IServices.Authentication;

namespace Artemis.Backend.Services.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            // For now, return the password as-is to match the current implementation
            // TODO: Implement proper password hashing in the future
            return password;
        }

        public bool VerifyPassword(string password, string hash)
        {
            // For now, do direct comparison to match the current implementation
            // TODO: Implement proper password verification in the future
            return password == hash;
        }
    }
}
