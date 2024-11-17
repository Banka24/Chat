using ClientApp.Infrastructure;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand OpenConnectionWindowCommand { get; }
        public ICommand OpenServerFavoriteCommand { get; }
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