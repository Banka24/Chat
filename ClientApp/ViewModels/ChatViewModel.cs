using CommunityToolkit.Mvvm.Input;
using Chat.Client;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.ObjectModel;

namespace ClientApp.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private string _serverName = string.Empty;
        private string _userMessage = string.Empty;

        public IChatClient ChatClient { get; set; }
        public IRelayCommand SendMessageCommand { get; }

        public string ServerName
        {
            get => _serverName;
            set => SetProperty(ref _serverName, value);
        }

        public ObservableCollection<string> Messages { get; } = [];

        public string UserMessage
        {
            get => _userMessage;
            set => SetProperty(ref _userMessage, value.Trim());
        }

        public ChatViewModel(IChatClient chatClient)
        {
            SendMessageCommand = new RelayCommand(async () => await SendMessage());
            ChatClient = chatClient;
            ChatClient.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(string message)
        {
            Messages.Add(message);
            OnPropertyChanged(nameof(Messages));
        }

        private async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(UserMessage))
            {
                await ChatClient.SendAsync(Encoding.UTF8.GetBytes(UserMessage), CancellationToken.None);

                Messages.Add($"Я: {UserMessage}");
                UserMessage = string.Empty;
            }
        }
    }
}