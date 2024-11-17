using ClientApp.Infrastructure;
using ClientApp.Models.Entity;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class FavoriteServersViewModel : ViewModelBase
    {
        private Server _selectedServer = null!;
        private ICollection<Server> _servers = null!;
        public ICommand ConnectionCommand { get; }
        public ICollection<Server> Servers
        {
            get => _servers;
            set => SetProperty(ref _servers, value);
        }
        public Server Server
        {
            get => _selectedServer;
            set => SetProperty(ref _selectedServer, value);
        }
        public FavoriteServersViewModel()
        {
            ConnectionCommand = new RelayCommand(ExecuteConnection);
            _ = Task.Run(async () => Servers = await LoadServers());
        }

        private void ExecuteConnection()
        {
            LocalStorage.IpAdress = Server.IpAdress;
            NavigationService.NavigateTo(new ConnectionViewModel());
        }

        private static async Task<ICollection<Server>> LoadServers()
        {
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