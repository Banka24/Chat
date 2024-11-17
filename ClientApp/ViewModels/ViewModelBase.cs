using ClientApp.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly INavigationService NavigationService;
        public ICommand GoBackCommand { get; }
        protected ViewModelBase()
        {
            NavigationService = App.ServiceProvider.GetService<INavigationService>()!;
            GoBackCommand = new RelayCommand(ExecuteBack);
        }

        private void ExecuteBack()
        {
            NavigationService.GoBack();
        }
    }
}