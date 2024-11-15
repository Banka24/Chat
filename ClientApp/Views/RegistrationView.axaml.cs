using Avalonia.Controls;
using ClientApp.ViewModels;

namespace ClientApp.Views;

public partial class RegistrationView : UserControl
{
    public RegistrationView()
    {
        InitializeComponent();
        DataContext = new RegistrationViewModel();
    }
}