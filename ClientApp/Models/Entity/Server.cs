namespace ClientApp.Models.Entity
{
    public class Server(string? name, string ipAdress, string password)
    {
        public string? Name { get; set; } = name;
        public string IpAdress { get; set; } = ipAdress;
        public string Password { get; set; } = password;
    }
}