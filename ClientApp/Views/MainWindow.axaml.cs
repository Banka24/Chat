using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ClientApp.Views
{
    /// <summary>
    /// Главное окно приложения.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="MainWindow"/>.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Обрабатывает событие загрузки окна.
        /// </summary>
        /// <param name="e">Событие загрузки.</param>
        protected override void OnLoaded(RoutedEventArgs e)
        {
            // Получаем ширину и высоту рабочей области основного экрана
            int screenWidth = Screens.Primary!.WorkingArea.Width;
            int screenHeight = Screens.Primary!.WorkingArea.Height;

            // Устанавливаем ширину и высоту окна в 95% от размеров экрана
            Width = screenWidth * 0.95;
            Height = screenHeight * 0.95;

            // Устанавливаем позицию окна в центре экрана
            Position = new PixelPoint((int)((screenWidth - Width) / 2), (int)((screenHeight - Height) / 2));

            // Вызываем базовую реализацию метода
            base.OnLoaded(e);
        }
    }
}