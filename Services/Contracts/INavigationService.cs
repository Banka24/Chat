using ClientApp.ViewModels;

namespace ClientApp.Contracts
{
    public interface INavigationService
    {
        void LoadStartViewModel(ViewModelBase viewModel);
        void NavigateTo(ViewModelBase view);
        void GoBack();
    }
}