using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Абстрактный класс ViewModelBase представляет базовую модель представления.
    /// </summary>
    public abstract class ViewModelBase : ObservableObject, IViewModel
    {
        /// <summary>
        /// Сервис навигации.
        /// </summary>
        protected readonly INavigationService NavigationService;

        /// <summary>
        /// Команда для возврата на предыдущий экран.
        /// </summary>
        public ICommand GoBackCommand { get; }

        /// <summary>
        /// Текущая модель представления.
        /// </summary>
        public virtual INotifyPropertyChanged CurrentViewModel { get; set; } = null!;

        /// <summary>
        /// Конструктор класса ViewModelBase.
        /// Инициализирует сервис навигации и команду для возврата на предыдущий экран.
        /// </summary>
        protected ViewModelBase()
        {
            NavigationService = App
                .ServiceProvider
                .GetService<INavigationService>()!;

            GoBackCommand = new RelayCommand(ExecuteBack);
        }

        /// <summary>
        /// Метод для выполнения возврата на предыдущий экран.
        /// Переходит на предыдущий экран.
        /// </summary>
        protected virtual void ExecuteBack()
        {
            NavigationService.GoBack();
        }
    }
}