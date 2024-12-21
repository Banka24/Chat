namespace Chat.ClientApp.Models
{
    /// <summary>
    /// Класс для работы с сервером чата.
    /// </summary>
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