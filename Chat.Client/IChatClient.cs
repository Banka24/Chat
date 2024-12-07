namespace Chat.Client
{
    public interface IChatClient
    {
        public string ServerName { get; }
        Task<bool> ConnectAsync(string ipAddress, string password, string userName, CancellationToken cancellationToken);
        Task SendAsync(byte[] data, CancellationToken cancellationToken);
        Task<string> GetServerNameAsync(CancellationToken cancellationToken);
        public event Action<string> MessageReceived;
    }
}