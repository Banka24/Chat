using ClientApp.Views;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public ICommand OpenConnectionWindowCommand { get; }
        public HomeViewModel()
        {
            OpenConnectionWindowCommand = new RelayCommand(ExecuteOpenConnectionWindow);
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
    }
}