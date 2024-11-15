using ClientApp.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace ClientApp.ViewModels
{
    public class RegistrationViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private string _login = string.Empty;
        private string _password = string.Empty;

        public ICommand RegistrationCommand { get; }
        public ICommand GoBackCommand { get; }

        public string Login
        {
            get => _login;
            set => SetProperty(ref _login, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Error => null!;

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case nameof(Login):
                        if (string.IsNullOrWhiteSpace(Login))
                            return "Заполните поле Логин";
                        break;
                    case nameof(Password):
                        if (string.IsNullOrWhiteSpace(Password) || Password.Length < 8)
                        {
                            return "Пароль должен содержать как минимум 8 символов.";
                        }

                        if (!Password.Any(char.IsDigit))
                        {
                            return "Пароль должен содержать хотя бы одну цифру.";
                        }

                        if (!Password.Any(char.IsUpper))
                        {
                            return "Пароль должен содержать хотя бы одну заглавную букву.";
                        }

                        if (!Password.Any(char.IsLower))
                        {
                            return "Пароль должен содержать хотя бы одну строчную букву.";
                        }

                        if (!Password.Any(ch => !char.IsLetterOrDigit(ch)))
                        {
                            return "Пароль должен содержать хотя бы один специальный символ.";
                        }
                        break;
                }
                return null!;
            }
        }

        public RegistrationViewModel() : base()
        {
            _securityService = App.ServiceProvider.GetService<ISecurityService>()!;
            _userService = App.ServiceProvider.GetRequiredService<IUserService>();
            RegistrationCommand = new RelayCommand(ExecuteRegistration);
            GoBackCommand = new RelayCommand(ExecuteBack);
        }

        private void ExecuteRegistration()
        {
            if (Error.Length != 0) return;
            string password = _securityService.HashPasswordUser(Password);
            bool registrationStatus = _userService.Registration(Login, password);


        }

        private void ExecuteBack()
        {
            NavigationService.GoBack();
        }
    }
}