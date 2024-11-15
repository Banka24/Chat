using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class StartView : UserControl
{
    public StartView()
    {
        InitializeComponent();
        DataContext = new StartViewModel();
    }
}