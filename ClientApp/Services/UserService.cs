using ClientApp.Contracts;
using ClientApp.Models.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ClientApp.Services
{
    public class UserService : IUserService
    {
        private const string PATH = @"~\Infrastructure\UserData.json";
        public User Authorization(string login, string password)
        {
            if (File.Exists(PATH))
            {
                try
                {
                    string jsonContent = File.ReadAllText(PATH);
                    var users = JsonConvert.DeserializeObject<IEnumerable<User>>(jsonContent) ?? Enumerable.Empty<User>();
                    var user = users.FirstOrDefault(u => u.Login == login && u.Password == password);
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

        public bool Registration(string login, string password)
        {
            var user = new User(login, password);
            string json = JsonConvert.SerializeObject(user, Formatting.Indented);
            try
            {
                File.WriteAllText(PATH, json);
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