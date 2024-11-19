using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using System.ComponentModel;

namespace Chat.ClientApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<IViewModel> _viewModels = new();
        private IViewModel _startViewModel = null!;

        public void LoadStartViewModel(IViewModel viewModel)
        {
            if (_startViewModel == null)
            {
                _startViewModel = viewModel;
                _viewModels.Push(_startViewModel);
            }
        }

        public void GoBack()
        {
            if (_viewModels.Count <= 1) return;
            _viewModels.Pop();
            _startViewModel.CurrentViewModel = (INotifyPropertyChanged)_viewModels.Peek();
        }

        public void NavigateTo(IViewModel viewModel)
        {
            _viewModels.Push(viewModel);
            if (_startViewModel == null) return;
            _startViewModel.CurrentViewModel = (INotifyPropertyChanged)_viewModels.Peek();
        }
    }
}