using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Diagnostics.Debug;

namespace Chat.Client
{
    public class ChatClient : IChatClient
    {
        private string _userName = string.Empty;
        private Socket _clientSocket = null!;

        public event Action<string> MessageReceived = null!;

        public async Task<bool> ConnectAsync(string ipAddress, string userName, CancellationToken cancellationToken)
        {
            _userName = userName;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                await _clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(ipAddress), 8888), cancellationToken);
                WriteLine("Подключено к серверу");

                var nameBytes = Encoding.UTF8.GetBytes(_userName);
                await SendAsync(nameBytes, cancellationToken);

                _ = ReceiveMessagesAsync(cancellationToken);
                return true;
            }
            catch (SocketException ex)
            {
                WriteLine($"Ошибка подключения: {ex.Message}");
                return false;
            }
            catch (FormatException ex)
            {
                WriteLine(ex.Message);
                return false;
            }
        }

        public async Task SendAsync(byte[] data, CancellationToken cancellationToken)
        {
            try
            {
                await _clientSocket.SendAsync(data, SocketFlags.None, cancellationToken);
            }
            catch (SocketException ex)
            {
                WriteLine($"Ошибка отправки данных: {ex.Message}");
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int received = await _clientSocket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
                    if (received == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    MessageReceived?.Invoke(message);
                }
                catch (SocketException ex)
                {
                    WriteLine($"Ошибка получения данных: {ex.Message}");
                    break;
                }
            }
        }
    }
}