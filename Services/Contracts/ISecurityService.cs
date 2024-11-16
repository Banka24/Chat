namespace ClientApp.Contracts
{
    public interface ISecurityService
    {
        string HashPasswordUser(string inputPassword);
        bool VerifyUser(string inputPassword, string hashPassword);
    }
}