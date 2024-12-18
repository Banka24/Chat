using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Chat.Client;
using Chat.ClientApp.DTO;
using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
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

        private string _serverName = string.Empty;
        private string _userMessage = string.Empty;
        private bool _isReadOnly = false;
        private ICollection<IStorageFile> _files = null!;
        private readonly IMessageFormatter _messageFormatter;
        private readonly IChatClient _chatClient;
        private readonly IZipService _zipService;

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

            SendMessageCommand = new RelayCommand(async () => await SendMessage());
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

        private async Task SendMessage()
        {
            string userName = App
                .ServiceProvider
                .GetRequiredService<User>()
                .Login;

            if (_files?.Count > 0)
            {
                await SendFilesAsync(userName);
            }
            else if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                await SendTextMessageAsync(userName, UserMessage);
            }
        }

        private async Task SendFilesAsync(string userName)
        {
            foreach (IStorageFile file in _files)
            {
                try
                {
                    byte[] fileBytes = await ReadFileAsBytes(file);
                    byte[] compresFile = _zipService.CompressFile(fileBytes);
                    
                    // Формируем текстовое сообщение с указанием типа
                    string messageToSend = _messageFormatter.SerializeMessage(userName, MessageTypeImage, compresFile);

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

        private async Task SendTextMessageAsync(string userName, string message)
        {
            // Формируем текстовое сообщение с указанием типа
            string messageToSend = _messageFormatter.SerializeMessage(userName, MessageTypeString, message);

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

        private static async Task<byte[]> ReadFileAsBytes(IStorageFile file)
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
    }
}
