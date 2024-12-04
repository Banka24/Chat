namespace Chat.Client
{
    public interface IChatClient
    {
        Task<bool> ConnectAsync(string ipAddress, string userName, CancellationToken cancellationToken);
        Task SendAsync(byte[] data, CancellationToken cancellationToken);
        public event Action<string> MessageReceived;
    }
}
