using System.Collections.Generic;

namespace Chat.ClientApp.DTO
{
    public record class AudioMessage(string UserName, ICollection<byte> Audio);
}