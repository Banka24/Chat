namespace Chat.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();
            var server = new ChatServer();

            var serverTask = server.StartServerAsync("26.202.40.211", 8888, cts.Token);
            Console.WriteLine("Нажмите Enter для остановки сервера...");
            Console.ReadLine();

            cts.Cancel();
            await serverTask;
        }
    }
}