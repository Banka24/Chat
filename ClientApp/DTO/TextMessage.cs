namespace Chat.ClientApp.DTO
{
    /// <summary>
    /// Представляет текстовое сообщение, содержащее текст.
    /// </summary>
    /// <param name="Text">Текст сообщения.</param>
    public record class TextMessage(string Text);
}