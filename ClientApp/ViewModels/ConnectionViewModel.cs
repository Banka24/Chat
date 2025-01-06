using Chat.Client;
using Chat.ClientApp.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Представляет модель представления для подключения.
    /// </summary>
    public class ConnectionViewModel : ViewModelBase
    {
        private string _ipAddress = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;
        private bool _isConnecting;

        /// <summary>
        /// Получает или задает состояние подключения.
        /// </summary>
        public bool IsConnecting
        {
            get => _isConnecting;
            set => SetProperty(ref _isConnecting, value);
        }

        /// <summary>
        /// Получает команду для выполнения подключения.
        /// </summary>
        public IRelayCommand ConnectionCommand { get; }

        /// <summary>
        /// Получает или задает сообщение об ошибке.
        /// </summary>
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        /// <summary>
        /// Получает или задает IP-адрес.
        /// </summary>
        public string IpAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }

        /// <summary>
        /// Получает или задает пароль.
        /// </summary>
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса ConnectionViewModel.
        /// </summary>
        public ConnectionViewModel()
        {
            ConnectionCommand = new RelayCommand(async () => await ExecuteConnectionCommand());
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса ConnectionViewModel с указанным IP-адресом.
        /// </summary>
        /// <param name="ipAddress">IP-адрес.</param>
        public ConnectionViewModel(string ipAddress) : this()
        {
            IpAddress = ipAddress;
        }

        /// <summary>
        /// Выполняет команду подключения.
        /// </summary>
        private async Task ExecuteConnectionCommand()
        {
            using var cancellationTokenSource = new CancellationTokenSource();

            var chatClient = App
                .ServiceProvider
                .GetRequiredService<IChatClient>();

            var login = App
                .ServiceProvider
                .GetRequiredService<User>()
                .Login;

            IsConnecting = true;

            ErrorMessage = "Подключение";
            bool isConnected = await chatClient.ConnectAsync(IpAddress, Password, login, cancellationTokenSource.Token);
            if (isConnected)
            {
                var chatViewModel = new ChatViewModel(chatClient)
                {
                    ServerName = chatClient.ServerName,
                };

                NavigationService.NavigateTo(chatViewModel);
            }
            else
            {
                ErrorMessage = "Произошла ошибка подключения";
                await Task.Delay(3000);
                ErrorMessage = string.Empty;
            }

            IsConnecting = false;
        }
    }
}