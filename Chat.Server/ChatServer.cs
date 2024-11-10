using System.Net.Sockets;
using System.Net;

namespace Chat.Server
{
    public static class ChatServer
    {
        private static readonly List<ClientHandler> _clients = [];
        public static async Task StartServerAsync(string address, int port, CancellationToken token)
        {
            var ip = IPAddress.Parse(address);
            var listener = new TcpListener(ip, port);
            listener.Start();
            Console.WriteLine("Сервер запущен...");
            while (true)
            {
                var client = await listener.AcceptTcpClientAsync(token);
                var clientHandler = new ClientHandler(client);
                _clients.Add(clientHandler);
                _ = clientHandler.HandleClientAsync(_clients);
            }
        }
    }
}