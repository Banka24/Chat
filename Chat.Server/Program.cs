namespace Chat.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            var serverTask = ChatServer.StartServerAsync("127.0.0.1", 8888, cts.Token);
            Console.WriteLine("Нажмите Enter для остановки сервера...");
            Console.ReadLine();

            cts.Cancel();
            await serverTask;
        }
    }
}