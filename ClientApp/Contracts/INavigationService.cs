using ClientApp.ViewModels;

namespace ClientApp.Contracts
{
    public interface INavigationService
    {
        ViewModelBase NavigateTo(ViewModelBase view);
        ViewModelBase GoBack();
    }
}