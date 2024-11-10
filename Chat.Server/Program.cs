using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

class ChatServer
{
    private static Socket _serverSocket;
    private static readonly byte[] _buffer = new byte[1024];

    static void Main(string[] args)
    {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(new IPEndPoint(IPAddress.Any, 8888));
        _serverSocket.Listen(10);
        Console.WriteLine("Сервер запущен. Ожидание подключений");

        while (true)
        {
            Socket clientSocket = _serverSocket.Accept();
            Console.WriteLine("Клиент подключен.");
            Thread clientThread = new Thread(HandleClient);
            clientThread.Start(clientSocket);
        }
    }

    private static void HandleClient(object obj)
    {
        Socket clientSocket = (Socket)obj;

        while (true)
        {
            int received = clientSocket.Receive(_buffer);
            if (received == 0) break;

            string message = Encoding.UTF8.GetString(_buffer, 0, received);
            Console.WriteLine("Сообщение от клиента: " + message);

            // Ответ клиенту (эхо)
            clientSocket.Send(_buffer, 0, received, SocketFlags.None);
        }

        clientSocket.Close();
        Console.WriteLine("Клиент отключен.");
    }
}
