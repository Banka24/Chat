using System.Net.Sockets;

namespace Chat.ClientApp
{
    public static class LocalStorage
    {
        public static string Login { get; set; } = string.Empty;
        public static string IpAdress { get; set; } = string.Empty;
        public static Socket Socket { get; set; } = null!;
    }
}