using System.ComponentModel;

namespace ClientApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private INotifyPropertyChanged _currentViewModel = null!;
        public override INotifyPropertyChanged CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value, nameof(CurrentViewModel));
        }

        public MainWindowViewModel()
        {
            NavigationService.LoadStartViewModel(this);
            NavigationService.NavigateTo(new StartViewModel());
        }
    }
}