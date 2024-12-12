using Avalonia.Platform.Storage;
using Chat.Client;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

        /// <summary>
        /// Получает коллекцию сообщений.
        /// </summary>
        public ObservableCollection<string> Messages { get; } = [];

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
        private void OnMessageReceived(string message)
        {
            Messages.Add(message);
            OnPropertyChanged(nameof(Messages));
        }

        /// <summary>
        /// Отправляет сообщение через клиента чата.
        /// </summary>
        private async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                await ChatClient.SendAsync(Encoding.UTF8.GetBytes(UserMessage), CancellationToken.None);

                Messages.Add($"Я: {UserMessage}");
                UserMessage = string.Empty;
            }
        }

        public async Task AddFile(IReadOnlyList<IStorageFile> files)
        {
            ICollection<byte> fileBytes;

            foreach (var file in files)
            {
                // Получаем массив байтов из файла
                fileBytes = await ReadFileAsBytes(file);
                await ChatClient.SendAsync([..fileBytes], CancellationToken.None);
            }
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