using Chat.ClientApp.Models;

namespace Chat.ClientApp.Services.Contracts
{
    public interface INavigationService
    {
        void LoadStartViewModel(IViewModel viewModel);
        void NavigateTo(IViewModel view);
        void GoBack();
    }
}