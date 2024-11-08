using System.Net.Sockets;
using System.Text;

namespace Chat.Client
{
    public class Program
    {
        private static string _userName = string.Empty;

        public static void Main(string[] args)
        {
            Console.Write("Введите ваше имя: ");
            _userName = Console.ReadLine() ?? "Неизвестный";
            Work().Wait();
        }

        private static async Task Work()
        {
            using var client = new TcpClient();
            try
            {
                await client.ConnectAsync("26.202.40.211", 8888);

                var stream = client.GetStream();
                var nameBytes = Encoding.UTF8.GetBytes(_userName);
                await stream.WriteAsync(nameBytes);

                _ = Task.Run(async () => await ReadMessagesAsync(stream));
                while (true)
                {
                    var message = Console.ReadLine() ?? throw new ArgumentNullException();
                    var bytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(bytes);
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Такого адреса нет");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadLine();
            }
        }

        private static async Task ReadMessagesAsync(NetworkStream stream)
        {
            var buffer = new byte[512];
            while (true)
            {
                try
                {
                    var bytesRead = await stream.ReadAsync(buffer);
                    if (bytesRead == 0) break;
                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка при чтении сообщения: " + e.Message);
                    break;
                }
            }
        }
    }
}