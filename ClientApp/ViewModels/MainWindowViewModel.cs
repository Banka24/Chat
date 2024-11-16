namespace ClientApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel = null!;
        public ViewModelBase CurrentViewModel
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