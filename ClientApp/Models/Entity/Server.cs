using System;

namespace ClientApp.Models.Entity
{
    public class Server(string? name, string ipAdress)
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Name { get; set; } = name;
        public string IpAdress { get; set; } = ipAdress;
    }
}