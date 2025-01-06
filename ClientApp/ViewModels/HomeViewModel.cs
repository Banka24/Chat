using Chat.ClientApp.Models;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Класс HomeViewModel представляет модель представления для главной страницы.
    /// </summary>
    public class HomeViewModel : ViewModelBase
    {
        /// <summary>
        /// Команда для открытия окна подключения.
        /// </summary>
        public IRelayCommand OpenConnectionWindowCommand { get; }

        /// <summary>
        /// Команда для открытия окна избранного сервера.
        /// </summary>
        public IRelayCommand OpenServerFavoriteCommand { get; }

        /// <summary>
        /// Команда для открытия окна запуска сервера.
        /// </summary>
        public IRelayCommand OpenStartServerCommand { get; }

        /// <summary>
        /// Свойство для получения логина пользователя.
        /// </summary>
        public string Login { get; private set; }

        /// <summary>
        /// Конструктор класса HomeViewModel.
        /// Инициализирует экземпляры команд и устанавливает логин пользователя.
        /// </summary>
        public HomeViewModel()
        {
            Login = App
                .ServiceProvider
                .GetRequiredService<User>()
                .Login;

            OpenConnectionWindowCommand = new RelayCommand(ExecuteOpenConnection);
            OpenServerFavoriteCommand = new RelayCommand(ExecuteOpenServerFavorite);
            OpenStartServerCommand = new RelayCommand(ExecuteOpenStartServer);
        }

        /// <summary>
        /// Метод для выполнения команды открытия окна подключения.
        /// Переход к новому представлению ConnectionViewModel.
        /// </summary>
        private void ExecuteOpenConnection()
        {
            NavigationService.NavigateTo(new ConnectionViewModel());
        }

        /// <summary>
        /// Метод для выполнения команды открытия окна запуска сервера.
        /// Переход к новому представлению ServerStartViewModel.
        /// </summary>
        private void ExecuteOpenStartServer()
        {
            NavigationService.NavigateTo(new ServerStartViewModel());
        }

        /// <summary>
        /// Метод для выполнения команды открытия окна избранного сервера.
        /// Переход к новому представлению FavoriteServersViewModel.
        /// </summary>
        private void ExecuteOpenServerFavorite()
        {
            NavigationService.NavigateTo(new FavoriteServersViewModel());
        }
    }
}