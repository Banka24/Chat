using ClientApp.Models.Entity;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class FavoriteServersViewModel : ViewModelBase
    {
        private Server _selectedServer = null!;
        public ObservableCollection<Server> Servers { get; set; } = null!;

        public ICommand ConnectionCommand { get; }

        public Server Server
        {
            get => _selectedServer;
            set => SetProperty(ref _selectedServer, value);
        }

        public FavoriteServersViewModel()
        {
            ConnectionCommand = new RelayCommand(ExecuteConnection);
            _ = Task.Run(async () => Servers = (ObservableCollection<Server>)await LoadServers());
        }

        private void ExecuteConnection()
        {
            System.Diagnostics.Debug.WriteLine("Идёт подключение");
            return;
        }

        private static async Task<ICollection<Server>> LoadServers()
        {
            string path = @"D:\learn\C#\Chat\ClientApp\Infrastructure\Servers.json";
            if (File.Exists(path))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(path);
                    var serversCollection = JsonConvert.DeserializeObject<ServersCollection>(jsonContent);
                    return serversCollection?.Servers ?? null!;
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