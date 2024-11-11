using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Chat.Client
{
    public class ChatClient
    {
        private static string _userName = string.Empty;
        private static Socket _clientSocket = null!;

        static async Task Main(string[] args)
        {
            Console.Write("Введите ваше имя: ");
            _userName = Console.ReadLine() ?? "Неизвестный";

            await StartWorkAsync();
        }

        private async static Task StartWorkAsync()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            using (_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    await _clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse("26.202.40.211"), 8888), cancellationTokenSource.Token);
                    Console.WriteLine("Подключено к серверу");

                    var nameBytes = Encoding.UTF8.GetBytes(_userName);
                    await SendAsync(nameBytes, cancellationTokenSource.Token);

                    _ = ReceiveMessagesAsync(cancellationTokenSource.Token);

                    while (true)
                    {
                        Console.Write("Я: ");
                        string message = Console.ReadLine()!;
                        if (string.IsNullOrWhiteSpace(message)) continue;

                        await SendAsync(Encoding.UTF8.GetBytes(message), cancellationTokenSource.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Операция отменена.");
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Ошибка подключения: {ex.Message}");
                }
            }
        }

        private async static Task SendAsync(byte[] data, CancellationToken cancellationToken)
        {
            try
            {
                await _clientSocket.SendAsync(data, SocketFlags.None, cancellationToken);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка отправки данных: {ex.Message}");
            }
        }

        private async static Task ReceiveMessagesAsync(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int received = await _clientSocket.ReceiveAsync(buffer, SocketFlags.None, cancellationToken);
                    if (received == 0) break;

                    string message = Encoding.UTF8.GetString(buffer, 0, received);
                    Console.WriteLine("Сообщение от сервера: " + message);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine($"Ошибка получения данных: {ex.Message}");
                    break;
                }
            }
        }
    }
}