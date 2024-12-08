using Chat.Server;
using System.Threading.Tasks;

namespace Chat.ClientApp
{
    public static class ServerManager
    {
        private static IChatServer _chatServer = null!;

        public static async Task StartServer(string serverName, string serverPassword)
        {
            if (_chatServer == null)
            {
                _chatServer = new ChatServer();
                await _chatServer
                    .InsertServerSettingsAsync(serverName, serverPassword)
                    .ConfigureAwait(false);
            }
        }

        public static void StopServer()
        {
            if (_chatServer != null)
            {
                _chatServer.Stop();
                _chatServer = null!;
            }
        }
    }
}