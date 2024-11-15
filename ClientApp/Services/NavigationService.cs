using ClientApp.Contracts;
using ClientApp.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

namespace ClientApp.Services
{
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly Stack<ViewModelBase> _viewModels = new();

        public ViewModelBase GoBack()
        {
            if(_viewModels.Count <= 0) return
            _viewModels.Pop();
            return _viewModels.Peek();
        }

        public ViewModelBase NavigateTo(ViewModelBase view)
        {
            _viewModels.Push(view);
            return view;
        }
    }
}