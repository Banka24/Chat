using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClientApp.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnLoaded(RoutedEventArgs e)
        {
            int screenWidth = Screens.Primary!.WorkingArea.Width;
            int screenHeight = Screens.Primary!.WorkingArea.Height;
            Width = screenWidth * 0.95;
            Height = screenHeight * 0.95;
            Position = new PixelPoint((int)((screenWidth - Width) / 2), (int)((screenHeight - Height) / 2));
            base.OnLoaded(e);
        }
    }
}