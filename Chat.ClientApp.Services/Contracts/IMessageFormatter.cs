using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface IMessageFormatter
    {
        string SerializeMessage(string userName, string type, string inputMessage);
        SocketMessage DeserializeMessage(string jsonMessage);
    }
}