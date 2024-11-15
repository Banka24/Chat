using ClientApp.Views;

namespace ClientApp.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel() : base()
        {
            CurrentViewModel = new StartView();
        }
    }
}