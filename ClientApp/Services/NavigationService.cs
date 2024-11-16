using ClientApp.Contracts;
using ClientApp.ViewModels;
using System.Collections.Generic;

namespace ClientApp.Services
{
    public class NavigationService : INavigationService
    {
        private readonly Stack<ViewModelBase> _viewModels = new();
        private MainWindowViewModel _startViewModel = null!;

        public void LoadStartViewModel(ViewModelBase viewModel)
        {
            if (_startViewModel == null && viewModel is MainWindowViewModel mainViewModel)
            {
                _startViewModel = mainViewModel;
                _viewModels.Push(_startViewModel);
            }
        }

        public void GoBack()
        {
            if (_viewModels.Count <= 0) return;
            _viewModels.Pop();
            _startViewModel.CurrentViewModel = _viewModels.Peek();
        }

        public void NavigateTo(ViewModelBase viewModel)
        {
            _viewModels.Push(viewModel);
            if (_startViewModel == null) return;
            _startViewModel.CurrentViewModel = _viewModels.Peek();
        }
    }
}