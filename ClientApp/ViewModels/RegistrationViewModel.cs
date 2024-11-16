using ClientApp.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using ValidationRules;

namespace ClientApp.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _message = string.Empty;
        private string _messageColor = string.Empty;

        public ICommand RegistrationCommand { get; }
        public ICommand GoBackCommand { get; }

        [Required]
        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        public string MessageColor
        {
            get => _messageColor;
            set => SetProperty(ref _messageColor, value);
        }

        [ValidPassword]
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public RegistrationViewModel() : base()
        {
            _securityService = App.ServiceProvider.GetService<ISecurityService>()!;
            _userService = App.ServiceProvider.GetRequiredService<IUserService>();
            RegistrationCommand = new RelayCommand(ExecuteRegistration);
            GoBackCommand = new RelayCommand(ExecuteBack);
        }

        private async void ExecuteRegistration()
        {
            string password = _securityService.HashPasswordUser(Password);
            bool registrationStatus = await _userService.RegistrationAsync(Login, password);
            if (registrationStatus)
            {
                MessageColor = "Green";
                Message = "Аккаунт зарегестрирован";
            }
            else
            {
                MessageColor = "Red";
                Message = "Произошла ошибка";
            }
        }

        private void ExecuteBack()
        {
            NavigationService.GoBack();
        }
    }
}