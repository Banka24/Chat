using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    /// <summary>
    /// Класс MessageFormatter реализует интерфейс IMessageFormatter и представляет методы для форматирования сообщений в JSON для передачи по сети.
    /// </summary>
    public class MessageFormatter : IMessageFormatter
    {
        /// <summary>
        /// Сериализует сообщение в формат JSON.
        /// </summary>
        /// <typeparam name="T">Тип сообщения.</typeparam>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="type">Тип сообщения.</param>
        /// <param name="inputMessage">Сообщение для сериализации.</param>
        /// <returns>Сериализованное сообщение в формате JSON.</returns>
        public string SerializeMessage<T>(string userName, string type, T inputMessage)
        {
            var message = new SocketMessage<T>(userName, type, inputMessage);
            return JsonConvert.SerializeObject(message);
        }

        /// <summary>
        /// Десериализует сообщение из формата JSON.
        /// </summary>
        /// <typeparam name="T">Тип сообщения.</typeparam>
        /// <param name="jsonMessage">Сообщение в формате JSON для десериализации.</param>
        /// <returns>Десериализованное сообщение.</returns>
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