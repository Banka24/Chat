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
        /// Метод LoadServers асинхронно загружает коллекцию серверов из JSON-файла.
        /// </summary>
        /// <returns>Возвращает коллекцию серверов или null, если файл не существует или произошла ошибка при чтении или десериализации.</returns>
        public async Task<ICollection<Server>> LoadServers()
        {
            string path = AppDomain
                .CurrentDomain
                .BaseDirectory
                .ToString() + "Servers.json";

            if (File.Exists(path))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(path);
                    var serversCollection = JsonConvert.DeserializeObject<ICollection<Server>>(jsonContent);
                    return serversCollection ?? null!;
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла: {ex.Message}");
                    return null!;
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка десериализации: {ex.Message}");
                    return null!;
                }
            }

            return null!;
        }
    }
}