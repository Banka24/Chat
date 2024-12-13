using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using Chat.Client;
using Chat.ClientApp.DTO;
using Chat.ClientApp.Models;
using CommunityToolkit.Mvvm.Input;
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
        private string _serverName = string.Empty;
        private string _userMessage = string.Empty;
        private bool _isReadOnly = false;
        private ICollection<IStorageFile> _files = null!;

        /// <summary>
        /// Получает или задает клиента чата.
        /// </summary>
        public IChatClient ChatClient { get; set; }

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
            SendMessageCommand = new RelayCommand(async () => await SendMessage());
            ChatClient = chatClient;
            ChatClient.MessageReceived += OnMessageReceived;
        }

        /// <summary>
        /// Обрабатывает получение сообщения от клиента чата.
        /// </summary>
        /// <param name="message">Полученное сообщение.</param>
        private void OnMessageReceived(object message)
        {
            Messages.Add(message);
            OnPropertyChanged(nameof(Messages));
        }

        private Bitmap CreateImage(byte[] bytes)
        {
            using var ms = new MemoryStream(bytes);
            return new Bitmap(ms);
        }

        /// <summary>
        /// Отправляет сообщение через клиента чата.
        /// </summary>
        private async Task SendMessage()
        {
            if (_files?.Count > 0)
            {
                foreach (var file in _files)
                {
                    var fileBytes = await ReadFileAsBytes(file);
                    await ChatClient.SendAsync(fileBytes, CancellationToken.None);
                    var image = CreateImage(fileBytes);
                    Messages.Add(new ImageMessage(image, image.PixelSize.Width, image.PixelSize.Height));
                }

                UserMessage = string.Empty;
                IsReadOnly = false;
                _files.Clear();
            }
            else if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                var textMessageBytes = Encoding.UTF8.GetBytes(UserMessage);
                await ChatClient.SendAsync(textMessageBytes, CancellationToken.None);
                Messages.Add(new TextMessage($"Я: {UserMessage}"));
                UserMessage = string.Empty;
            }
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

            foreach (var file in fileNames)
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
    }
}
