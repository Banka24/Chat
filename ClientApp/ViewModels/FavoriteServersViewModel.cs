﻿using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Представляет модель представления для страницы "Избранные серверы".
    /// </summary>
    public class FavoriteServersViewModel : ViewModelBase
    {
        /// <summary>
        /// Сервис сервера.
        /// </summary>
        private readonly IServerService _serverService;

        /// <summary>
        /// Выбранный сервер.
        /// </summary>
        private Server _selectedServer = null!;

        /// <summary>
        /// Коллекция серверов.
        /// </summary>
        private ICollection<Server> _servers = null!;

        /// <summary>
        /// Команда подключения.
        /// </summary>
        public IRelayCommand ConnectionCommand { get; }

        /// <summary>
        /// Коллекция серверов.
        /// </summary>
        public ICollection<Server> Servers
        {
            get => _servers;
            set => SetProperty(ref _servers, value);
        }

        /// <summary>
        /// Выбранный сервер.
        /// </summary>
        public Server Server
        {
            get => _selectedServer;
            set => SetProperty(ref _selectedServer, value);
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="FavoriteServersViewModel"/>.
        /// </summary>
        public FavoriteServersViewModel()
        {
            _serverService = App
                .ServiceProvider
                .GetRequiredService<IServerService>();

            ConnectionCommand = new RelayCommand(ExecuteConnection);
            Servers = _serverService.LoadServers().Result;
        }

        /// <summary>
        /// Выполняет команду подключения к выбранному серверу.
        /// </summary>
        private void ExecuteConnection()
        {
            if (Server != null)
            {
                NavigationService.NavigateTo(new ConnectionViewModel(Server.IpAdress));
            }
        }
    }
}