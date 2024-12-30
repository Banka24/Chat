using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    /// <summary>
    /// Класс ServerService реализует интерфейс IServerService и предоставляет метод LoadServers для загрузки коллекции серверов из JSON-файла.
    /// </summary>
    public class ServerService : IServerService
    {
        /// <summary>
        /// Путь к файлу, где хранятся данные о серверах.
        /// </summary>
        private readonly string _path = Path
            .Combine(
                AppDomain
                .CurrentDomain
                .BaseDirectory, "Servers.json"
            );

        /// <summary>
        /// Коллекция серверов.
        /// </summary>
        private ICollection<Server> _servers = [];

        /// <summary>
        /// Добавляет новый сервер.
        /// </summary>
        /// <param name="name">Имя сервера.</param>
        /// <param name="ipAddress">IP-адрес сервера.</param>
        /// <returns>True, если сервер успешно добавлен, иначе false.</returns>
        public async Task<bool> AddServerAsync(string name, string ipAddress)
        {
            await ReadServersAsync();

            var newServer = new Server(name, ipAddress);
            _servers.Add(newServer);

            return await WriteServersAsync(_servers);
        }

        /// <summary>
        /// Загружает список серверов.
        /// </summary>
        /// <returns>Коллекция серверов.</returns>
        public async Task<ICollection<Server>> LoadServers()
        {
            await ReadServersAsync();
            return _servers;
        }

        /// <summary>
        /// Читает данные о серверах из файла.
        /// </summary>
        private async Task ReadServersAsync()
        {
            if (!File.Exists(_path))
            {
                _servers = [];
                return;
            }

            try
            {
                string json = await File.ReadAllTextAsync(_path);
                _servers = JsonConvert.DeserializeObject<ICollection<Server>>(json) ?? [];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                _servers = [];
            }
        }

        /// <summary>
        /// Записывает данные о серверах в файл.
        /// </summary>
        /// <param name="servers">Коллекция серверов.</param>
        /// <returns>True, если данные успешно записаны, иначе false.</returns>
        private async Task<bool> WriteServersAsync(ICollection<Server> servers)
        {
            string json = JsonConvert.SerializeObject(servers, Formatting.Indented);

            try
            {
                await File.WriteAllTextAsync(_path, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}