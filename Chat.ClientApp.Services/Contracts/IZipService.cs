namespace Chat.ClientApp.Services.Contracts
{
    public interface IZipService
    {
        public byte[] CompressFile(byte[] data);
        public byte[] DecompressFile(byte[] compressedData);
    }
}
