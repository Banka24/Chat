using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistartionCommand { get; }

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