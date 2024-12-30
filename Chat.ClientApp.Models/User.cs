namespace Chat.ClientApp.Models
{
    /// <summary>
    /// Класс для работы с пользователем чата.
    /// </summary>
    /// <remarks>
    /// Инициализирует новый экземпляр класса User.
    /// </remarks>
    /// <param name="login">Логин пользователя.</param>
    /// <param name="password">Пароль пользователя.</param>
    public class User(string login, string password)
    {
        /// <summary>
        /// Логин пользователя.
        /// </summary>
        public string Login { get; set; } = login;

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; } = password;
    }
}