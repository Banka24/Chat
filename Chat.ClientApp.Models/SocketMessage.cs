namespace Chat.ClientApp.Models
{
    /// <summary>
    /// Представляет сообщение, отправленное через сокет.
    /// </summary>
    /// <typeparam name="T">Тип сообщения.</typeparam>
    public record class SocketMessage<T>
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Тип сообщения.
        /// </summary>
        public string Type { get; init; }

        /// <summary>
        /// Сообщение.
        /// </summary>
        public T Message { get; init; }

        /// <summary>
        /// Инициализирует новый экземпляр класса SocketMessage.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="type">Тип сообщения.</param>
        /// <param name="message">Сообщение.</param>
        public SocketMessage(string userName, string type, T message)
        {
            Name = userName;
            Type = type;
            Message = message;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SocketMessage без параметров.
        /// </summary>
        public SocketMessage()
        {

        }
    }
}