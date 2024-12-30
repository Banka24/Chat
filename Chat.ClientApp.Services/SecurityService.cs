using Chat.ClientApp.Services.Contracts;
using static BCrypt.Net.BCrypt;

namespace Chat.ClientApp.Services
{
    /// <summary>
    /// Класс SecurityService реализует интерфейс ISecurityService и предоставляет методы для для работы с безопасностью пользователей.
    /// </summary>
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// Метод для хеширования пароля пользователя.
        /// </summary>
        /// <param name="inputPassword">Входящий пароль пользователя.</param>
        /// <returns>Хешированный пароль пользователя.</returns>
        public string HashPasswordUser(string inputPassword)
        {
            return HashPassword(inputPassword);
        }

        /// <summary>
        /// Метод для проверки пользователя по хешу пароля.
        /// </summary>
        /// <param name="inputPassword">Входящий пароль пользователя.</param>
        /// <param name="hashPassword">Хеш пароля пользователя.</param>
        /// <returns>Результат проверки пользователя.</returns>
        public bool VerifyUser(string inputPassword, string hashPassword)
        {
            return Verify(inputPassword, hashPassword);
        }
    }
}