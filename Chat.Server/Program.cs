using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat.Server
{
    public class ChatServer
    {
        private readonly static List<Socket> _connectedClients = [];

        static async Task Main(string[] args)
        {
            using var serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
            serverSocket.Listen(10);
            Console.WriteLine("Сервер запущен. Ожидание подключений");

            while (true)
            {
                var clientSocket = await serverSocket.AcceptAsync();
                Console.WriteLine("Клиент подключен.");
                _ = HandleClientAsync(clientSocket);
            }
        }

        private async static Task HandleClientAsync(Socket clientSocket)
        {
            _connectedClients.Add(clientSocket);

            byte[] buffer = new byte[1024];
            string userName = string.Empty;

            try
            {
                while (true)
                {
                    int received = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                    if (received == 0) break;

                    if (string.IsNullOrEmpty(userName))
                    {
                        userName = Encoding.UTF8.GetString(buffer, 0, received);
                        Console.WriteLine($"Пользователь присоединился: {userName}");
                        continue;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine($"Сообщение от {userName}: " + message);

                    foreach (var otherClient in _connectedClients)
                    {
                        if (otherClient != clientSocket)
                        {
                            string formattedMessage = $"{userName}: {message}";
                            byte[] messageBytes = Encoding.UTF8.GetBytes(formattedMessage);
                            await otherClient.SendAsync(messageBytes, SocketFlags.None);
                        }
                    }
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
            }
        }
    }
}