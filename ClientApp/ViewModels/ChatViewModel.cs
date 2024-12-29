using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Chat.Client;
using Chat.ClientApp.DTO;
using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp.ViewModels
{
    /// <summary>
    /// Представляет модель представления для чата.
    /// </summary>
    public class ChatViewModel : ViewModelBase
    {
        private const string MessageTypeString = "str";
        private const string MessageTypeImage = "img";
        private const string MessageTypeAudio = "aud";

        private string _serverName = string.Empty;
        private string _userMessage = string.Empty;
        private bool _isReadOnly = false;
        private ICollection<IStorageFile> _files = null!;
        private readonly IMessageFormatter _messageFormatter;
        private readonly IChatClient _chatClient;
        private readonly IZipService _zipService;
        private readonly string _userName;

        /// <summary>
        /// Получает команду для отправки сообщения.
        /// </summary>
        public IRelayCommand SendMessageCommand { get; }

        /// <summary>
        /// Получает или задает имя сервера.
        /// </summary>
        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set => SetProperty(ref _isReadOnly, value);
        }

        /// <summary>
        /// Получает коллекцию сообщений.
        /// </summary>
        public ObservableCollection<object> Messages { get; } = [];

        /// <summary>
        /// Получает или задает сообщение пользователя.
        /// </summary>
        public string UserMessage
        {
            get => _userMessage;
            set => SetProperty(ref _userMessage, value.Trim());
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса ChatViewModel.
        /// </summary>
        /// <param name="chatClient">Клиент чата.</param>
        public ChatViewModel(IChatClient chatClient)
        {
            _messageFormatter = App
                .ServiceProvider
                .GetRequiredService<IMessageFormatter>();

            _zipService = App
                .ServiceProvider
                .GetRequiredService<IZipService>();

            _userName = App
                .ServiceProvider
                .GetRequiredService<User>()
                .Login;

            SendMessageCommand = new RelayCommand(async () => await SendMessageAsync());
            _chatClient = chatClient;
            _chatClient.MessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// Обрабатывает получение сообщения от клиента чата.
        /// </summary>
        /// <param name="message">Полученное сообщение.</param>
        private void OnMessageReceived(string message)
        {
            var data = _messageFormatter.DeserializeMessage<object>(message);

            switch (data.Type)
            {
                case MessageTypeImage:

                    byte[] message64 = Convert.FromBase64String(data.Message.ToString()!);
                    byte[] decomposeImage = _zipService.DecompressFile(message64);

                    Messages.Add(data.Name);
                    ShowImage(decomposeImage);
                    break;

                case MessageTypeString:
                    var textMessage = new TextMessage(data.Message.ToString()!);
                    Messages.Add($"{data.Name}: {textMessage.Text}");
                    break;

                case MessageTypeAudio:
                    byte[] audioBytes = ((JArray)data.Message).Select(b => (byte)(int)b).ToArray();
                    var audioMessage = new AudioMessage(data.Name, audioBytes);
                    Messages.Add(audioMessage);
                    break;

                default:
                    System.Diagnostics.Debug.WriteLine("Другой тип");
                    break;
            }
        }

        private static Bitmap CreateImage(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        private async Task SendMessageAsync()
        {
            if (_files?.Count > 0)
            {
                await SendFilesAsync();
            }
            else if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                await SendTextMessageAsync(UserMessage);
            }
        }

        private async Task SendFilesAsync()
        {
            foreach (IStorageFile file in _files)
            {
                try
                {
                    byte[] fileBytes = await ReadFileAsBytesAsync(file);
                    byte[] compresFile = _zipService.CompressFile(fileBytes);

                    // Формируем текстовое сообщение с указанием типа
                    string messageToSend = _messageFormatter.SerializeMessage(_userName, MessageTypeImage, compresFile);

                    var messageBytes = Encoding
                        .UTF8
                        .GetBytes(messageToSend);

                    await _chatClient.SendAsync(messageBytes, CancellationToken.None);

                    ShowImage(fileBytes);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error sending file {file.Name}: {ex.Message}");
                }
            }

            UserMessage = string.Empty;
            IsReadOnly = false;
            _files.Clear();
        }

        private async Task SendTextMessageAsync(string message)
        {
            // Формируем текстовое сообщение с указанием типа
            string messageToSend = _messageFormatter.SerializeMessage(_userName, MessageTypeString, message);

            var textMessageBytes = Encoding
                .UTF8
                .GetBytes(messageToSend);

            await _chatClient.SendAsync(textMessageBytes, CancellationToken.None);

            Messages.Add(new TextMessage($"Я: {message}"));
            UserMessage = string.Empty;
        }

        public void AddSelectedFiles(IReadOnlyList<IStorageFile> files)
        {
            _files = [.. files];
            IsReadOnly = true;

            PrintFileNames(
                _files.Select(file => file.Name)
            );
        }

        private void PrintFileNames(IEnumerable<string> fileNames)
        {
            var sb = new StringBuilder();

            foreach (string file in fileNames)
            {
                sb.AppendLine(file);
            }

            UserMessage = sb.ToString();
        }

        private static async Task<byte[]> ReadFileAsBytesAsync(IStorageFile file)
        {
            using var stream = await file.OpenReadAsync();
            using var memoryStream = new MemoryStream();

            await stream.CopyToAsync(memoryStream);

            return memoryStream.ToArray();
        }

        private void ShowImage(byte[] imageByteArray)
        {
            var image = CreateImage(imageByteArray);
            var imageMessage = new ImageMessage(image, image.PixelSize.Width, image.PixelSize.Height);
            Messages.Add(imageMessage);
        }

        public async Task SendAudioAsync(ICollection<byte> buffer)
        {
            var messageToSend = _messageFormatter.SerializeMessage(_userName, MessageTypeAudio, buffer);

            var messageBytes = Encoding
                       .UTF8
                       .GetBytes(messageToSend);

            await _chatClient.SendAsync(messageBytes, CancellationToken.None);
            Messages.Add(new AudioMessage("Я:", buffer));
        }
    }
}