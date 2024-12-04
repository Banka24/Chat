using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public abstract class ViewModelBase : ObservableObject, IViewModel
    {
        protected readonly INavigationService NavigationService;
        public ICommand GoBackCommand { get; }
        public virtual INotifyPropertyChanged CurrentViewModel { get; set; } = null!;

        protected ViewModelBase()
        {
            NavigationService = App
                .ServiceProvider
                .GetService<INavigationService>()!;

            GoBackCommand = new RelayCommand(ExecuteBack);
        }

        protected virtual void ExecuteBack()
        {
            NavigationService.GoBack();
        }
    }
}