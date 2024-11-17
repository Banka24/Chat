using ClientApp.Models.Entity;
using System.Threading.Tasks;

namespace ClientApp.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserInfoAsync(string login);
        Task<bool> RegistrationAsync(string login, string password);
    }
}