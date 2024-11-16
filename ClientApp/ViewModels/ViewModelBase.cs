using ClientApp.Contracts;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.ViewModels
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly INavigationService NavigationService;
        protected ViewModelBase()
        {
            NavigationService = App.ServiceProvider.GetService<INavigationService>()!;
        }
    }
}