namespace Chat.ClientApp.Models
{
    public record class SocketMessage(string UserName, string Type, string Message);
}