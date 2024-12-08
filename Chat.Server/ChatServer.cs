using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat.Server
{
    public class ChatServer : IChatServer
    {
        private readonly ConcurrentDictionary<Socket, string> _connectedClients = [];
        private string _serverName = string.Empty;
        private string _serverPassword = string.Empty;
        private bool _isRunning = false;

        public bool IsRunning => _isRunning;

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

        public void Stop()
        {
            _isRunning = false;
        }

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

        private async Task HandleClientAsync(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
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

                    string formattedMessage = $"{userName}: {message}";

                    await SendMessageToOthersAsync(clientSocket, formattedMessage)
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

        private async Task NotifyClientsAsync(string message)
        {
            byte[] messageBytes = Encoding
                .UTF8
                .GetBytes(message);

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

        private bool CheckAccess(string password)
        {
            return password == _serverPassword;
        }
    }
}