using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Chat.Server
{
    public class ChatServer : IChatServer
    {
        private readonly List<Socket> _connectedClients = [];
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
            await StartWorkAsync();
        }

        public void Stop()
        {
            if (_isRunning)
            {
                _isRunning = false; 
            }
        }

        private async Task StartWorkAsync()
        {
            using var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
            serverSocket.Listen(10);
            Console.WriteLine("Сервер запущен. Ожидание подключений");

            while (_isRunning)
            {
                var clientSocket = await serverSocket.AcceptAsync();
                _ = HandleClientAsync(clientSocket);
            }
        }

        private async Task HandleClientAsync(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            string userName = string.Empty;

            try
            {
                int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                userName = Encoding.UTF8.GetString(buffer, 0, received);

                received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                string enteredPassword = Encoding.UTF8.GetString(buffer, 0, received);

                if (!CheckAccess(enteredPassword))
                {
                    await clientSocket.SendAsync(Encoding.UTF8.GetBytes("Неверный пароль. Подключение закрыто."), SocketFlags.None);
                    clientSocket.Close();
                    return;
                }

                await clientSocket.SendAsync(Encoding.UTF8.GetBytes(_serverName), SocketFlags.None);

                await NotifyClientsAsync($"{userName} присоединился к чату.");
                _connectedClients.Add(clientSocket);

                while (true)
                {
                    received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    if (received == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    string formattedMessage = $"{userName}: {message}";

                    await SendMessageToOthersAsync(clientSocket, formattedMessage);
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
            }
            finally
            {
                clientSocket.Close();
                _connectedClients.Remove(clientSocket);
                Console.WriteLine("Клиент отключен.");
                await NotifyClientsAsync($"{userName} покинул чат.");
            }
        }

        private async Task NotifyClientsAsync(string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            foreach (var client in _connectedClients)
            {
                await client.SendAsync(messageBytes, SocketFlags.None);
            }
        }

        private async Task SendMessageToOthersAsync(Socket sender, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            foreach (var otherClient in _connectedClients)
            {
                if (otherClient != sender)
                {
                    await otherClient.SendAsync(messageBytes, SocketFlags.None);
                }
            }
        }

        private bool CheckAccess(string password)
        {
            return password == _serverPassword;
        }
    }
}