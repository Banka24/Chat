using CommunityToolkit.Mvvm.Input;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.Collections;
using ClientApp.Infrastructure;
using Chat.ClientApp.Services.Contracts;
using Chat.ClientApp.ValidationRules;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    public class LoginViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private readonly Dictionary<string, List<string>> _errors = [];
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _message = string.Empty;
        private string _messageColor = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IRelayCommand LoginCommand { get; }

        [ValidLogin]
        public string Login
        {
            get => _login;
            set
            {
                SetProperty(ref _login, value);
                ValidateProperty(nameof(Login));
            }
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
            set
            {
                SetProperty(ref _password, value);
                ValidateProperty(nameof(Password));
            }

        }

        public bool HasErrors => _errors.Count > 0;

        public LoginViewModel()
        {
            _securityService = App
                .ServiceProvider
                .GetService<ISecurityService>()!;
            _userService = App
                .ServiceProvider
                .GetRequiredService<IUserService>();
            LoginCommand = new RelayCommand(async () => await ExecuteLogin());
        }

        private async Task ExecuteLogin()
        {
            var user = await _userService.GetUserInfoAsync(Login);

            if (user == null || user.Login != Login)
            {
                SetErrorMessage();
                return;
            }

            bool verify = _securityService.VerifyUser(Password, user.Password);

            if (verify)
            {
                LocalStorage.Login = user.Login;
                NavigationService.NavigateTo(new HomeViewModel());
                return;
            }

            SetErrorMessage();
        }

        private void SetErrorMessage()
        {
            MessageColor = "Red";
            Message = "Неверен логин или пароль";
        }

        private void ValidateProperty(string propertyName)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(this) { MemberName = propertyName };

            Validator.TryValidateProperty(
                GetType()
                .GetProperty(propertyName)?
                .GetValue(this),
                validationContext, validationResults
                );

            if (validationResults.Count > 0)
            {
                _errors[propertyName] = validationResults
                    .Select(v => v.ErrorMessage)
                    .Where(error => error != null)
                    .Cast<string>()
                    .ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (propertyName != null && _errors.TryGetValue(propertyName, out var errors))
            {
                return errors;
            }
            return Array.Empty<string>();
        }
    }
}