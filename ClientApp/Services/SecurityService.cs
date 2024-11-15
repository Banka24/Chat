using ClientApp.Contracts;
using static BCrypt.Net.BCrypt;

namespace ClientApp.Services
{
    public class SecurityService : ISecurityService
    {
        public string HashPasswordUser(string inputPassword)
        {
            return HashPassword(inputPassword);
        }

        public bool VerifyUser(string inputPassword, string hashPassword)
        {
            return Verify(inputPassword, hashPassword);
        }
    }
}