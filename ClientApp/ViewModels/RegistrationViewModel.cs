using Chat.ClientApp.Services.Contracts;
using Chat.ClientApp.ValidationRules;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Класс модели представления для регистрации.
    /// </summary>
    public class RegistrationViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private readonly ISecurityService _securityService;
        private readonly IUserService _userService;
        private readonly Dictionary<string, List<string>> _errors = [];
        private string _login = string.Empty;
        private string _password = string.Empty;
        private string _message = string.Empty;
        private string _messageColor = string.Empty;

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        public IRelayCommand RegistrationCommand { get; }

        /// <summary>
        /// Свойство для получения и установки логина.
        /// </summary>
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

        /// <summary>
        /// Свойство для получения и установки пароля.
        /// </summary>
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

        public RegistrationViewModel()
        {
            _securityService = App
                .ServiceProvider
                .GetService<ISecurityService>()!;

            _userService = App
                .ServiceProvider
                .GetRequiredService<IUserService>();

            RegistrationCommand = new RelayCommand(async () => await ExecuteRegistration());
        }

        private async Task ExecuteRegistration()
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