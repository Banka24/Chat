using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    public class MessageFormatter : IMessageFormatter
    {
        public string SerializeMessage(string userName, string type, string inputMessage)
        {
            var message = new SocketMessage(userName, type, inputMessage);
            return JsonConvert.SerializeObject(message);
        }

        public SocketMessage DeserializeMessage(string jsonMessage)
        {
            var message = new SocketMessage(string.Empty, string.Empty, string.Empty);

            try
            {
                message = JsonConvert.DeserializeObject<SocketMessage>(jsonMessage)!;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return message;
        }
    }
}