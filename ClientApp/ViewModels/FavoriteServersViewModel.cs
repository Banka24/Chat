using Chat.ClientApp;
using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class FavoriteServersViewModel : ViewModelBase
    {
        private readonly IServerService _serverService;
        private Server _selectedServer = null!;
        private ICollection<Server> _servers = null!;
        public IRelayCommand ConnectionCommand { get; }
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
            _serverService = App
                .ServiceProvider
                .GetRequiredService<IServerService>();

            ConnectionCommand = new RelayCommand(ExecuteConnection);
            _ = Task.Run(async () => Servers = await _serverService.LoadServers());
        }

        private void ExecuteConnection()
        {
            if (Server != null)
            {
                LocalStorage.IpAdress = Server.IpAdress;
                NavigationService.NavigateTo(new ConnectionViewModel(Server.IpAdress));
            }
        }

        protected override void ExecuteBack()
        {
            LocalStorage.IpAdress = string.Empty;
            base.ExecuteBack();
        }
    }
}