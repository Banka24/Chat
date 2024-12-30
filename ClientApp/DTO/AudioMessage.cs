using System.Collections.Generic;

namespace Chat.ClientApp.DTO
{
    /// <summary>
    /// Представляет аудиосообщение, содержащее имя пользователя и аудиоданные.
    /// </summary>
    /// <param name="UserName">Имя пользователя, отправившего сообщение.</param>
    /// <param name="Audio">Аудиоданные сообщения.</param>
    public record class AudioMessage(string UserName, ICollection<byte> Audio);
}