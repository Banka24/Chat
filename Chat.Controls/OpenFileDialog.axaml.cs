using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Chat.Controls;

public partial class OpenFileDialog : Window
{
    public string SelectedFilePath { get; private set; } = null!;
    public OpenFileDialog()
    {
        InitializeComponent();
    }
    private async void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "Выберите файл",
            Filters = new List<OpenFileDialog>
                {
                    new FileDialogFilter
                    {
                        Name = "Текстовые файлы",
                        Extensions = { "txt", "md" }
                    },
                    new FileDialogFilter
                    {
                        Name = "Все файлы",
                        Extensions = { "*" }
                    }
                }
        };

        var result = await dialog.ShowAsync(this);
        if (result != null && result.Length > 0)
        {
            FilePathTextBox.Text = result[0];
        }
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        SelectedFilePath = FilePathTextBox.Text;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
}