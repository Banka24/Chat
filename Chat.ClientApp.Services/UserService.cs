using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    /// <summary>
    /// Класс UserService реализует интерфейс IUserService и предоставляет методы для получения информации о пользователе и регистрации нового пользователя.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly string _path = Path
            .Combine(
                AppDomain
                .CurrentDomain
                .BaseDirectory, "UserData.json"
            );

        /// <summary>
        /// Метод GetUserInfoAsync асинхронно получает информацию о пользователе по логину.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <returns>Возвращает объект User или null, если файл не существует или произошла ошибка при чтении или десериализации.</returns>
        public async Task<User> GetUserInfoAsync(string login)
        {
            if (File.Exists(_path))
            {
                try
                {
                    string jsonContent = await File.ReadAllTextAsync(_path);
                    var user = JsonConvert.DeserializeObject<User>(jsonContent);
                    return user ?? null!;
                }
                catch (IOException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка чтения файла: {ex.Message}");
                    return null!;
                }
                catch (JsonException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка десериализации: {ex.Message}");
                    return null!;
                }
            }

            return null!;
        }

        /// <summary>
        /// Метод RegistrationAsync асинхронно регистрирует нового пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращает true, если регистрация прошла успешно, и false в противном случае.</returns>
        public async Task<bool> RegistrationAsync(string login, string password)
        {
            var user = new User(login, password);
            string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            try
            {
                if (!File.Exists(_path))
                {
                    using var stream = File.Create(_path);
                }
                await File.WriteAllTextAsync(_path, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }

            return true;
        }
    }
}