using Chat.Server;
using ClientApp;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Chat.ClientApp
{
    /// <summary>
    /// Класс для управления сервером.
    /// </summary>
    public static class ServerManager
    {
        private static IChatServer _chatServer = null!;

        static ServerManager()
        {
            _chatServer = App
                .ServiceProvider
                .GetRequiredService<IChatServer>();
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
            }
        }
    }
}