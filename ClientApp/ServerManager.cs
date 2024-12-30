using Chat.Server;
using System.Threading.Tasks;

namespace Chat.ClientApp
{
    /// <summary>
    /// Класс для управления сервером.
    /// </summary>
    public static class ServerManager
    {
        /// <summary>
        /// Имя сервера.
        /// </summary>
        private static string _serverName;

        /// <summary>
        /// Пароль сервера.
        /// </summary>
        private static string _serverPassword;

        /// <summary>
        /// Флаг, указывающий, работает ли сервер.
        /// </summary>
        private static bool _isWork;

        /// <summary>
        /// Экземпляр чата сервера.
        /// </summary>
        private static IChatServer _chatServer = null!;

        /// <summary>
        /// Имя сервера.
        /// </summary>
        public static string ServerName => _serverName;

        /// <summary>
        /// Пароль сервера.
        /// </summary>
        public static string ServerPassword => _serverPassword;

        /// <summary>
        /// Флаг, указывающий, работает ли сервер.
        /// </summary>
        public static bool IsWork => _isWork;

        /// <summary>
        /// Конструктор класса ServerManager.
        /// </summary>
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

        /// <summary>
        /// Метод для изменения опций сервера.
        /// </summary>
        /// <param name="name">Имя сервера.</param>
        /// <param name="password">Пароль сервера.</param>
        /// <param name="work">Флаг, указывающий, работает ли сервер.</param>
        private static void ChangeOptions(string name, string password, bool work)
        {
            _serverName = name;
            _serverPassword = password;
            _isWork = work;
        }
    }
}