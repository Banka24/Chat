using CommunityToolkit.Mvvm.Input;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Класс StartViewModel представляет модель представления для начального экрана.
    /// </summary>
    public class StartViewModel : ViewModelBase
    {
        /// <summary>
        /// Команда для выполнения входа.
        /// </summary>
        public IRelayCommand LoginCommand { get; }

        /// <summary>
        /// Команда для выполнения регистрации.
        /// </summary>
        public IRelayCommand RegistrationCommand { get; }

        /// <summary>
        /// Конструктор класса StartViewModel.
        /// Инициализирует команды для выполнения входа и регистрации.
        /// </summary>
        public StartViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
            RegistrationCommand = new RelayCommand(ExecuteRegistration);
        }

        /// <summary>
        /// Метод для выполнения входа.
        /// Переходит на экран входа.
        /// </summary>
        private void ExecuteLogin()
        {
            NavigationService.NavigateTo(new LoginViewModel());
        }

        /// <summary>
        /// Метод для выполнения регистрации.
        /// Переходит на экран регистрации.
        /// </summary>
        private void ExecuteRegistration()
        {
            NavigationService.NavigateTo(new RegistrationViewModel());
        }
    }
}