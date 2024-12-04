using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using Newtonsoft.Json;

namespace Chat.ClientApp.Services
{
    public class UserService : IUserService
    {
        private readonly string _path = AppDomain.CurrentDomain.BaseDirectory.ToString() + "UserData.json";
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