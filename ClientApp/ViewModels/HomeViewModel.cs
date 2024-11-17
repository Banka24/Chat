using ClientApp.Infrastructure;
using ClientApp.Views;
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
            var connectionWidnow = new ConnectionWindowView();
            connectionWidnow.Show();
            if (!connectionWidnow.IsActive)
            {
                System.Diagnostics.Debug.WriteLine("Окно закрыто");
            }
        }

        private void ExecuteOpenServerFavorite()
        {
            NavigationService.NavigateTo(new FavoriteServersViewModel());
        }
    }
}