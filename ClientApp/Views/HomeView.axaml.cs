using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class HomeView : UserControl
{
    public HomeView()
    {
        InitializeComponent();
        DataContext = new HomeViewModel();
    }
}