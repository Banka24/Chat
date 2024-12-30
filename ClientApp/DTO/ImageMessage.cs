using Avalonia.Media.Imaging;

namespace Chat.ClientApp.DTO
{
    /// <summary>
    /// Представляет изображение, содержащее данные изображения, ширину и высоту.
    /// </summary>
    /// <param name="Image">Данные изображения.</param>
    /// <param name="Width">Ширина изображения.</param>
    /// <param name="Height">Высота изображения.</param>
    public record class ImageMessage(Bitmap Image, int Width, int Height);
}