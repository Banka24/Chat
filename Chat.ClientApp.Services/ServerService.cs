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
        private readonly string _path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Servers.json");
        private ICollection<Server> _servers = [];

        public async Task<bool> AddServerAsync(string name, string ipAddress)
        {
            await ReadServersAsync();
            var newServer = new Server(name, ipAddress);
            _servers.Add(newServer);
            return await WriteServersAsync(_servers);
        }

        public async Task<ICollection<Server>> LoadServers()
        {
            await ReadServersAsync();
            return _servers;
        }

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