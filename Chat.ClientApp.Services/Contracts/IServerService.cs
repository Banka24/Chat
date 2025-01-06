using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface IServerService
    {
        Task<bool> AddServerAsync(string name, string ipAddress);
        Task<ICollection<Server>> LoadServers();
    }
}