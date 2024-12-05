using Chat.ClientApp;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public IRelayCommand OpenConnectionWindowCommand { get; }
        public IRelayCommand OpenServerFavoriteCommand { get; }
        public string Login { get; private set; }
        public HomeViewModel()
        {
            Login = LocalStorage.Login;
            OpenConnectionWindowCommand = new RelayCommand(ExecuteOpenConnectionWindow);
            OpenServerFavoriteCommand = new RelayCommand(ExecuteOpenServerFavorite);
        }

        private void ExecuteOpenConnectionWindow()
        {
            NavigationService.NavigateTo(new ConnectionViewModel());
        }

        private void ExecuteOpenServerFavorite()
        {
            NavigationService.NavigateTo(new FavoriteServersViewModel());
        }
    }
}