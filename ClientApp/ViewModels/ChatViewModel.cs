using ClientApp.Infrastructure;

namespace ClientApp.ViewModels
{
    public class ChatViewModel : ViewModelBase
    {
        private string _serverName = string.Empty;
        public string ServerName
        { 
            get => _serverName;
            set => SetProperty(ref _serverName, value); 
        }

        public ChatViewModel()
        {
            
        }
    }
}
