using Chat.ClientApp.Services;
using Chat.ClientApp.Services.Contracts;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat.Server
{
    /// <summary>
    /// Класс ChatServer представляет собой сервер для чата.
    /// </summary>
    public class ChatServer : IChatServer
    {
        /// <summary>
        /// Словарь, содержащий подключенных клиентов и их имена.
        /// </summary>
        private readonly ConcurrentDictionary<Socket, string> _connectedClients = new();

        /// <summary>
        /// Имя сервера.
        /// </summary>
        private string _serverName = string.Empty;

        /// <summary>
        /// Пароль сервера.
        /// </summary>
        private string _serverPassword = string.Empty;

        /// <summary>
        /// Флаг, указывающий, запущен ли сервер.
        /// </summary>
        private bool _isRunning = false;

        /// <summary>
        /// Форматтер сообщений.
        /// </summary>
        private readonly IMessageFormatter _messageFormatter = new MessageFormatter();

        /// <summary>
        /// Свойство, указывающее, запущен ли сервер.
        /// </summary>
        public bool IsRunning => _isRunning;

        /// <summary>
        /// Метод InsertServerSettingsAsync устанавливает настройки сервера и запускает работу сервера.
        /// </summary>
        /// <param name="serverName">имя сервера</param>
        /// <param name="serverPassword">пароль сервера</param>
        /// <returns>true, если сервер успешно запущен, иначе false</returns>
        public async Task InsertServerSettingsAsync(string serverName, string serverPassword)
        {
            if (_isRunning) return;
            _serverName = serverName;
            _serverPassword = serverPassword;
            _isRunning = true;

            try
            {
                await StartWorkAsync()
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return;
            }
        }

        /// <summary>Метод Stop останавливает работу сервера.</summary>
        public void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Метод StartWorkAsync запускает работу сервера.
        /// </summary>
        private async Task StartWorkAsync()
        {
            using var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
            serverSocket.Listen(10);

            while (_isRunning)
            {
                var acceptTask = serverSocket.AcceptAsync();
                var timeoutTask = Task.Delay(
                    TimeSpan.FromSeconds(30)
                );

                var completedTask = await Task
                    .WhenAny(acceptTask, timeoutTask)
                    .ConfigureAwait(false);

                if (completedTask == timeoutTask) continue;

                var clientSocket = await acceptTask.ConfigureAwait(false);
                _ = HandleClientAsync(clientSocket);
            }
        }

        /// <summary>
        /// Метод HandleClientAsync обрабатывает подключенного клиента.
        /// </summary>
        /// <param name="clientSocket">сокет клиента</param>
        private async Task HandleClientAsync(Socket clientSocket)
        {
            byte[] buffer = new byte[27_000_000];
            string userName = string.Empty;

            try
            {
                int received = await clientSocket
                    .ReceiveAsync(buffer, SocketFlags.None)
                    .ConfigureAwait(false);

                userName = Encoding
                    .UTF8
                    .GetString(buffer, 0, received);

                received = await clientSocket
                    .ReceiveAsync(buffer, SocketFlags.None)
                    .ConfigureAwait(false);

                string enteredPassword = Encoding
                    .UTF8
                    .GetString(buffer, 0, received);

                if (!CheckAccess(enteredPassword))
                {
                    await clientSocket
                        .SendAsync(
                            Encoding
                            .UTF8
                            .GetBytes("Неверный пароль. Подключение закрыто."), SocketFlags.None
                        )
                        .ConfigureAwait(false);

                    clientSocket.Close();
                    return;
                }

                await clientSocket
                    .SendAsync(
                        Encoding
                        .UTF8
                        .GetBytes($"{_serverName}"), SocketFlags.None
                    )
                    .ConfigureAwait(false);

                _connectedClients.TryAdd(clientSocket, userName);

                await NotifyClientsAsync($"{userName} присоединился к чату.")
                    .ConfigureAwait(false);

                while (_isRunning)
                {
                    received = await clientSocket
                        .ReceiveAsync(buffer, SocketFlags.None)
                        .ConfigureAwait(false);

                    if (received == 0) break;

                    string message = Encoding
                        .UTF8
                        .GetString(buffer, 0, received);

                    await SendMessageToOthersAsync(clientSocket, message)
                        .ConfigureAwait(false);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
            }
            finally
            {
                _connectedClients.TryRemove(clientSocket, out _);

                clientSocket.Close();
                Console.WriteLine("Клиент отключен.");

                await NotifyClientsAsync($"{userName} покинул чат.")
                    .ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Метод NotifyClientsAsync уведомляет всех клиентов о событии.
        /// </summary>
        /// <param name="message">сообщение для уведомления</param>
        private async Task NotifyClientsAsync(string message)
        {
            string jsonMessage = _messageFormatter.SerializeMessage("Server", "str", message);

            byte[] messageBytes = Encoding
                .UTF8
                .GetBytes(jsonMessage);

            foreach (var client in _connectedClients.Keys)
            {
                try
                {
                    await client
                        .SendAsync(messageBytes, SocketFlags.None)
                        .ConfigureAwait(false);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Ошибка отправки сообщения клиенту: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Метод SendMessageToOthersAsync отправляет сообщение всем клиентам, кроме отправителя.
        /// </summary>
        /// <param name="sender">сокет отправителя</param>
        /// <param name="message">сообщение для отправки</param>
        private async Task SendMessageToOthersAsync(Socket sender, string message)
        {
            byte[] messageBytes = Encoding
                .UTF8
                .GetBytes(message);

            foreach (var otherClient in _connectedClients.Keys)
            {
                if (otherClient != sender)
                {
                    try
                    {
                        await otherClient
                            .SendAsync(messageBytes, SocketFlags.None)
                            .ConfigureAwait(false);
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"Ошибка отправки сообщения другому клиенту: {ex.Message}");
                    }
                }
            }
        }

        /// <summary>
        /// Метод CheckAccess проверяет доступ клиента по паролю.
        /// </summary>
        /// <param name="password">пароль клиента</param>
        /// <returns>true, если пароль верный, иначе false</returns>
        private bool CheckAccess(string password)
        {
            return password == _serverPassword;
        }
    }
}