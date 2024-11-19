namespace Chat.ClientApp.Models
{
    public class User(string login, string password)
    {
        public string Login { get; set; } = login;
        public string Password { get; set; } = password;
    }
}