using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    public class MessageFormatter : IMessageFormatter
    {
        public string SerializeMessage<T>(string userName, string type, T inputMessage)
        {
            var message = new SocketMessage<T>(userName, type, inputMessage);
            return JsonConvert.SerializeObject(message);
        }

        public SocketMessage<T> DeserializeMessage<T>(string jsonMessage)
        {
            var message = new SocketMessage<T>();

            try
            {
                message = JsonConvert.DeserializeObject<SocketMessage<T>>(jsonMessage)!;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return message;
        }
    }
}