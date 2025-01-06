using System.ComponentModel;
using System.Windows.Input;

namespace Chat.ClientApp.Models
{
    public interface IViewModel
    {
        public abstract INotifyPropertyChanged CurrentViewModel { get; set; }
        public ICommand GoBackCommand { get; }
    }
}