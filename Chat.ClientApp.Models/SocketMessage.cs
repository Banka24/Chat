namespace Chat.ClientApp.Models
{
    public record class SocketMessage<T>
    {
        public string Name { get; init; }
        public string Type { get; init; }
        public T Message { get; init; }

        public SocketMessage(string userName, string type, T message)
        {
            Name = userName;
            Type = type;
            Message = message;
        }

        public SocketMessage()
        {

        }
    }
}