using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface IServerService
    {
        Task<ICollection<Server>> LoadServers();
    }
}
