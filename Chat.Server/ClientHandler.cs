using System.Net.Sockets;
using System.Text;

namespace Chat.Server
{
    public class ClientHandler(TcpClient client)
    {
        private string _userName = string.Empty;
        private readonly TcpClient _client = client;
        private readonly NetworkStream _stream = client.GetStream();

        public async Task HandleClientAsync(List<ClientHandler> clients)
        {
            var buffer = new byte[256];
            int bytesRead = await _stream.ReadAsync(buffer);
            _userName = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

            while (true)
            {
                bytesRead = await _stream.ReadAsync(buffer);
                if (bytesRead == 0) break;
                var message = $"{_userName}: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}";
                Console.WriteLine(message);
                foreach (var client in clients)
                {
                    if (client != this)
                    {
                        var msgBytes = Encoding.UTF8.GetBytes(message);
                        await client._stream.WriteAsync(msgBytes);
                    }
                }
            }

            clients.Remove(this);
            _client.Close();
        }
    }
}
