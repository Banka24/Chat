using ClientApp.Models.Entity;

namespace ClientApp.Contracts
{
    public interface IUserService
    {
        User Authorization(string login, string password);
        bool Registration(string login, string password);
    }
}