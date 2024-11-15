using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class ConnectionWindowView : Window
{
    public ConnectionWindowView()
    {
        InitializeComponent();
        DataContext = new ConnectionWindowViewModel();
    }
}