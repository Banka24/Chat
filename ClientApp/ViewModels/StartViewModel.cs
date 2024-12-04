using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        public IRelayCommand LoginCommand { get; }
        public IRelayCommand RegistartionCommand { get; }

        public StartViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegistartionCommand = new RelayCommand(ExecuteRegistration);
        }

        private void ExecuteLogin()
        {
            NavigationService.NavigateTo(new LoginViewModel());
        }

        private void ExecuteRegistration()
        {
            NavigationService.NavigateTo(new RegistrationViewModel());
        }
    }
}