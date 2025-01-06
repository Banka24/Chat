using Chat.ClientApp.Services.Contracts;
using System.IO.Compression;

namespace Chat.ClientApp.Services
{
    /// <summary>
    /// Класс ZipService реализует интерфейс IZipService и представляет методы для сжатия и расжатия файлов.
    /// </summary>
    public class ZipService : IZipService
    {
        /// <summary>
        /// Сжимает файл.
        /// </summary>
        /// <param name="data">Данные файла для сжатия.</param>
        /// <returns>Сжатые данные файла.</returns>
        public byte[] CompressFile(byte[] data)
        {
            using var compressedStream = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                gzipStream.Write(data, 0, data.Length);
            }

            return compressedStream.ToArray();
        }

        /// <summary>
        /// Распаковывает файл.
        /// </summary>
        /// <param name="compressedData">Сжатые данные файла для распаковки.</param>
        /// <returns>Распакованные данные файла.</returns>
        public byte[] DecompressFile(byte[] compressedData)
        {
            using var compressedStream = new MemoryStream(compressedData);
            using var decompressedStream = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            {
                gzipStream.CopyTo(decompressedStream);
            }
            return decompressedStream.ToArray();
        }
    }
}