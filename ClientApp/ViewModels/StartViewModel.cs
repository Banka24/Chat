using ClientApp.Views;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class StartViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand RegistartionCommand { get; }

        public StartViewModel() : base()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegistartionCommand = new RelayCommand(ExecuteRegistration);
        }

        private void ExecuteLogin()
        {
            //CurrentViewModel = NavigationService.NavigateTo(new Login);
        }

        private void ExecuteRegistration()
        {
            CurrentViewModel = new RegistrationView();
        }
    }
}