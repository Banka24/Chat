using Chat.Server;
using System.Threading.Tasks;

namespace Chat.ClientApp
{
    /// <summary>
    /// Класс для управления сервером.
    /// </summary>
    public static class ServerManager
    {
        private static string _serverName;
        private static string _serverPassword;
        private static bool _isWork;

        private static IChatServer _chatServer = null!;
        public static string ServerName => _serverName;
        public static string ServerPassword => _serverPassword;
        public static bool IsWork => _isWork;

        static ServerManager()
        {
            _serverName = string.Empty;
            _serverPassword = string.Empty;
            _isWork = false;
        }

        /// <summary>
        /// Метод для запуска сервера.
        /// </summary>
        /// <param name="serverName">Имя сервера.</param>
        /// <param name="serverPassword">Пароль сервера.</param>
        public static async Task StartServer(string serverName, string serverPassword)
        {
            if (_chatServer == null)
            {
                ChangeOptions(serverName, serverPassword, true);

                _chatServer = new ChatServer();
                await _chatServer
                    .InsertServerSettingsAsync(serverName, serverPassword)
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Метод для остановки сервера.
        /// </summary>
        public static void StopServer()
        {
            if (_chatServer != null)
            {
                _chatServer.Stop();
                _chatServer = null!;

                ChangeOptions(string.Empty, string.Empty, false);
            }
        }

        private static void ChangeOptions(string name, string password, bool work)
        {
            _serverName = name;
            _serverPassword = password;
            _isWork = work;
        }
    }
}