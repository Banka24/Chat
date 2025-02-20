﻿using Chat.ClientApp;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Класс ViewModel для управления сервером.
    /// </summary>
    public class ServerStartViewModel : ViewModelBase
    {
        private string _message = string.Empty;
        private string _serverName;
        private string _serverPassword;
        private bool _serverWork;
        private bool _goBackEnabled = true;

        /// <summary>
        /// Команда для запуска сервера.
        /// </summary>
        public IRelayCommand StartCommand { get; }

        /// <summary>
        /// Команда для остановки сервера.
        /// </summary>
        public IRelayCommand StopCommand { get; }

        /// <summary>
        /// Сообщение, отображаемое в интерфейсе.
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// Имя сервера.
        /// </summary>
        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        /// <summary>
        /// Пароль сервера.
        /// </summary>
        public string ServerPassword
        {
            get => _serverPassword;
            set => SetProperty(ref _serverPassword, value);
        }

        /// <summary>
        /// Свойство, указывающее, работает ли сервер.
        /// </summary>
        public bool ServerWork
        {
            get => _serverWork;
            set => SetProperty(ref _serverWork, value);
        }

        public bool GoBackEnabled
        {
            get => _goBackEnabled;
            set => SetProperty(ref _goBackEnabled, value);
        }

        /// <summary>
        /// Конструктор класса ServerStartViewModel.
        /// Инициализирует команды для запуска и остановки сервера.
        /// </summary>
        public ServerStartViewModel()
        {
            StartCommand = new RelayCommand(async () => await StartServerExecute());
            StopCommand = new RelayCommand(async () => await StopServerExecute());
            _serverName = ServerManager.ServerName;
            _serverPassword = ServerManager.ServerPassword;
            _serverWork = ServerManager.IsWork;
        }

        private async Task StartServerExecute()
        {
            GoBackEnabled = false;
            ServerWork = true;
            await ShowMessage("Сервер запускается");
            GoBackEnabled = true;

            await ServerManager
               .StartServer(ServerName, ServerPassword)
               .ConfigureAwait(false);
        }

        private async Task StopServerExecute()
        {
            Message = "Остановка сервера";
            ServerManager.StopServer();
            await ShowMessage("Сервер остановлен.");
            ServerWork = false;
        }

        private async Task ShowMessage(string message)
        {
            Message = message;

            await Task
                .Delay(5000)
                .ConfigureAwait(false);

            Message = string.Empty;
        }
    }
}