namespace Chat.Server
{
    public interface IChatServer
    {
        public bool IsRunning { get; }
        public Task InsertServerSettingsAsync(string serverName, string serverPassword);
        public void Stop();
    }
}