using Chat.ClientApp.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public IRelayCommand OpenConnectionWindowCommand { get; }
        public IRelayCommand OpenServerFavoriteCommand { get; }
        public IRelayCommand OpenStartServerCommand { get; }
        public string Login { get; private set; }
        public HomeViewModel()
        {
            Login = App
                .ServiceProvider
                .GetRequiredService<User>()
                .Login;

            OpenConnectionWindowCommand = new RelayCommand(ExecuteOpenConnection);
            OpenServerFavoriteCommand = new RelayCommand(ExecuteOpenServerFavorite);
            OpenStartServerCommand = new RelayCommand(ExecuteOpenStartServer);
        }

        private void ExecuteOpenConnection()
        {
            NavigationService.NavigateTo(new ConnectionViewModel());
        }

        private void ExecuteOpenStartServer()
        {
            NavigationService.NavigateTo(new ServerStartViewModel());
        }

        private void ExecuteOpenServerFavorite()
        {
            NavigationService.NavigateTo(new FavoriteServersViewModel());
        }
    }
}