using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    public class ServerService : IServerService
    {
        public async Task<ICollection<Server>> LoadServers()
        {
            //string path = AppDomain
            //    .CurrentDomain
            //    .BaseDirectory
            //    .ToString() + "Servers.json";

            string path = @"D:\learn\C#\Chat\ClientApp\Infrastructure\Servers.json";
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
