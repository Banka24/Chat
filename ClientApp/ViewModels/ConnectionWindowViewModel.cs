using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    public class ConnectionWindowViewModel : ViewModelBase
    {
        private string _ipAdress = string.Empty;
        private string _password = string.Empty;

        public RelayCommand ConnectionCommand { get; }
        public RelayCommand<Window> GoBackCommand { get; }

        public string IpAdress
        {
            get => _ipAdress;
            set => SetProperty(ref _ipAdress, value);
        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ConnectionWindowViewModel()
        {
            ConnectionCommand = new RelayCommand(ExecuteConnectionCommand);
            GoBackCommand = new RelayCommand<Window>(ExecuteGoBackCommand!);
        }

        private void ExecuteGoBackCommand(Window window)
        {
            if (window is null) return;
            window.Close();
        }

        private void ExecuteConnectionCommand()
        {
            System.Diagnostics.Debug.WriteLine("Попытка подключиться");
        }
    }
}