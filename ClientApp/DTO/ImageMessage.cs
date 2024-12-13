using Avalonia.Media.Imaging;

namespace Chat.ClientApp.DTO
{
    public record class ImageMessage(Bitmap Image, int Width, int Height);
}