namespace Chat.ClientApp.Models
{
    /// <summary>
    /// Инициализирует новый экземпляр класса Server.
    /// </summary>
    /// <param name="name">Имя сервера.</param>
    /// <param name="ipAdress">IP-адрес сервера.</param>
    public class Server(string name, string ipAdress)
    {

        /// <summary>
        /// Имя сервера.
        /// </summary>
        public string? Name { get; set; } = name;

        /// <summary>
        /// IP-адрес сервера.
        /// </summary>
        public string IpAdress { get; set; } = ipAdress;
    }
}