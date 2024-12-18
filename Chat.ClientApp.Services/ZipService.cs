using Chat.ClientApp.Services.Contracts;
using System.IO.Compression;

namespace Chat.ClientApp.Services
{
    public class ZipService : IZipService
    {
        public byte[] CompressFile(byte[] data)
        {
            using var compressedStream = new MemoryStream();

            using (var gzipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                gzipStream.Write(data, 0, data.Length);
            }

            return compressedStream.ToArray();
        }

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