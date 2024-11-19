using ClientApp.Infrastructure;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class ConnectionViewModel : ViewModelBase
    {
        private string _ipAdress = string.Empty;
        private string _password = string.Empty;

        public ICommand ConnectionCommand { get; }

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

        public ConnectionViewModel()
        {
            IpAdress = LocalStorage.IpAdress;
            ConnectionCommand = new RelayCommand(ExecuteConnectionCommand);
        }

        private void ExecuteConnectionCommand()
        {
            NavigationService.NavigateTo(new ChatViewModel());
        }
    }
}