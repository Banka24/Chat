using Avalonia.Controls;
using ClientApp.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly INavigationService NavigationService;
        private UserControl _currentViewModel = null!;
        public UserControl CurrentViewModel
        {
            get => _currentViewModel;
            protected set => SetProperty(ref _currentViewModel, value);
        }

        protected ViewModelBase()
        {
            NavigationService = App.ServiceProvider.GetService<INavigationService>()!;
        }
    }
}