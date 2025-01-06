using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface IUserService
    {
        Task<User> GetUserInfoAsync(string login);
        Task<bool> RegistrationAsync(string login, string password);
    }
}