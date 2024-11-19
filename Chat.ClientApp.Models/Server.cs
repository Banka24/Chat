namespace Chat.ClientApp.Models
{
    public class Server(string name, string ipAdress)
    {
        public string? Name { get; set; } = name;
        public string IpAdress { get; set; } = ipAdress;
    }
}