using ClientApp.Models.Entity;
using System.Threading.Tasks;

namespace ClientApp.Contracts
{
    public interface IUserService
    {
        Task<User> Authorization(string login, string password);
        Task<bool> RegistrationAsync(string login, string password);
    }
}