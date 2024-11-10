class ChatClient
{
    private static Socket _clientSocket;

    static void Main(string[] args)
    {
        _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _clientSocket.Connect(new IPEndPoint(IPAddress.Parse("26.22.9.7"), 8888));
        Console.WriteLine("Подключено к серверу");

        Thread receiveThread = new Thread(ReceiveMessages);
        receiveThread.Start();

        while (true)
        {
            string message = Console.ReadLine();
            byte[] data = Encoding.UTF8.GetBytes(message);
            _clientSocket.Send(data);
        }
    }

    private static void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];

        while (true)
        {
            int received = _clientSocket.Receive(buffer);
            if (received == 0) break;

            string message = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine("Сообщение от сервера: " + message);
        }

        _clientSocket.Close();
        Console.WriteLine("Соединение с сервером закрыто.");
    }
}
