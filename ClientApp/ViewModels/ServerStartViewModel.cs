using Chat.ClientApp;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class ServerStartViewModel : ViewModelBase
    {
        private string _message = string.Empty;
        private string _serverName = string.Empty;
        private string _serverPassword = string.Empty;
        private bool _serverWork = false;
        public IRelayCommand StartCommand { get; }
        public IRelayCommand StopCommand { get; }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        public string ServerPassword
        {
            get => _serverPassword;
            set => SetProperty(ref _serverPassword, value);
        }
        public bool ServerWork
        {
            get => _serverWork;
            set => SetProperty(ref _serverWork, value);
        }

        public ServerStartViewModel()
        {
            StartCommand = new RelayCommand(async () => await StartServerExecute());
            StopCommand = new RelayCommand(async () => await StopServerExecute());
        }

        private async Task StartServerExecute()
        {
            await ServerManager.StartServer(ServerName, ServerPassword);
            await ShowMessage("Сервер уже запущен.");
            ServerWork = true;
        }

        private async Task StopServerExecute()
        {
            ServerManager.StopServer();
            await ShowMessage("Сервер остановлен.");
            ServerWork = false;
        }

        private async Task ShowMessage(string message)
        {
            Message = message;
            await Task.Delay(5000);
            Message = string.Empty;
        }
    }
}