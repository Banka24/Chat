using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface IMessageFormatter
    {
        string SerializeMessage<T>(string userName, string type, T inputMessage);
        SocketMessage<T> DeserializeMessage<T>(string jsonMessage);
    }
}