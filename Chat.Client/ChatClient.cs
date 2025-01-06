using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Diagnostics.Debug;

namespace Chat.Client
{
    /// <summary>
    /// Класс ChatClient представляет клиента чата, который может подключаться к серверу, отправлять и получать сообщения.
    /// </summary>
    public class ChatClient : IChatClient
    {
        private string _userName = string.Empty;
        private Socket _clientSocket = null!;
        private string _connectedServerName = string.Empty;
        private string _serverIpAddress = string.Empty;

        /// <summary>
        /// Получает имя сервера, к которому подключен клиент.
        /// </summary>
        public string ServerName => _connectedServerName;

        /// <summary>
        /// Получает IP-адрес сервера, к которому подключен клиент.
        /// </summary>
        public string IpAddress => _serverIpAddress;

        /// <summary>
        /// Событие, возникающее при получении сообщения от сервера.
        /// </summary>
        public event Action<string> MessageReceived = null!;

        /// <summary>
        /// Получает имя сервера, к которому подключен клиент.
        /// </summary>
        /// <returns>Имя сервера</returns>
        public async Task<string> GetServerNameAsync(CancellationToken cancellationToken)
        {
            byte[] serverNameBuffer = new byte[1024];
            int serverNameLength = await _clientSocket.ReceiveAsync(serverNameBuffer, SocketFlags.None, cancellationToken);

            return Encoding
                .UTF8
                .GetString(serverNameBuffer, 0, serverNameLength);
        }

        /// <summary>
        /// Подключает клиента к серверу по указанному IP-адресу и паролю.
        /// </summary>
        /// <param name="ipAddress">IP-адрес сервера</param>
        /// <param name="password">Пароль для подключения</param>
        /// <param name="userName">Имя пользователя</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <returns>True, если подключение успешно, иначе false</returns>
        public async Task<bool> ConnectAsync(string ipAddress, string password, string userName, CancellationToken cancellationToken)
        {
            _userName = userName;
            _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                var newIpAddress = IPAddress.Parse(ipAddress);
                _serverIpAddress = newIpAddress.ToString();
                await _clientSocket.ConnectAsync(new IPEndPoint(newIpAddress, 8888), cancellationToken);
                WriteLine("Подключено к серверу");

                var nameBytes = Encoding
                    .UTF8
                    .GetBytes(_userName);

                await SendAsync(nameBytes, cancellationToken);

                var passwordBytes = Encoding
                    .UTF8
                    .GetBytes(password);

                await SendAsync(passwordBytes, cancellationToken);

                byte[] responseBuffer = new byte[1024];
                int responseLength = await _clientSocket.ReceiveAsync(responseBuffer, SocketFlags.None, cancellationToken);

                string responseMessage = Encoding
                    .UTF8
                    .GetString(responseBuffer, 0, responseLength);

                if (responseMessage == "Неверный пароль. Подключение закрыто.")
                {
                    WriteLine("Ошибка: неверный пароль.");
                    _clientSocket.Close();
                    return false;
                }

                _connectedServerName = responseMessage;

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

        /// <summary>
        /// Отправляет данные на сервер.
        /// </summary>
        /// <param name="data">Данные для отправки</param>
        /// <param name="cancellationToken">Токен отмены</param>
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
            catch (OperationCanceledException)
            {
                WriteLine("Операция отправки была отменена.");
            }
        }

        private async Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[27_000_000];

            while (true)
            {
                try
                {
                    int received = await _clientSocket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
                    if (received == 0) break;

                    string data = Encoding
                        .UTF8
                        .GetString(buffer, 0, received);

                    MessageReceived?.Invoke(data);
                }
                catch (SocketException ex)
                {
                    WriteLine($"Ошибка получения данных: {ex.Message}");
                    break;
                }
                catch (FormatException ex)
                {
                    WriteLine($"Ошибка формата данных: {ex.Message}");
                    break;
                }
            }
        }
    }
}