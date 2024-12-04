﻿using CommunityToolkit.Mvvm.Input;
using System.Threading;
using System.Threading.Tasks;
using Chat.Client;
using Microsoft.Extensions.DependencyInjection;
using ClientApp.Infrastructure;

namespace ClientApp.ViewModels
{

    public class ConnectionViewModel : ViewModelBase
    {
        private string _ipAddress = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public IRelayCommand ConnectionCommand { get; }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public string IpAddress
        {
            get => _ipAddress;
            set => SetProperty(ref _ipAddress, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public ConnectionViewModel()
        {
            ConnectionCommand = new RelayCommand(async () => await ExecuteConnectionCommand());
        }

        private async Task ExecuteConnectionCommand()
        {
            using var cancellationTokenSource = new CancellationTokenSource();
            var chatClient = App.ServiceProvider.GetRequiredService<IChatClient>();

            bool isConnected = await chatClient.ConnectAsync(IpAddress, LocalStorage.Login, cancellationTokenSource.Token);
            if (isConnected)
            {
                var chatViewModel = new ChatViewModel(chatClient)
                {
                    ServerName = IpAddress,
                };

                NavigationService.NavigateTo(chatViewModel);
            }
            else
            {
                ErrorMessage = "Произошла ошибка подключения";
                await Task.Delay(3000);
                ErrorMessage = null!;
            }
        }
    }
}