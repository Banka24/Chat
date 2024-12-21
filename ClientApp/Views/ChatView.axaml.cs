using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ClientApp.Views;

public partial class ChatView : UserControl
{
    private readonly IStorageProvider _storageProvider;

    public ChatView()
    {
        _storageProvider = App.ServiceProvider.GetRequiredService<IStorageProvider>();
        InitializeComponent();
        AddButton.Click += AddFile_Click;
    }

    private async void AddFile_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var optionAllFiles = new FilePickerOpenOptions()
        {
            AllowMultiple = true,
            FileTypeFilter = [FilePickerFileTypes.ImageAll]
        };

        var files = await _storageProvider.OpenFilePickerAsync(optionAllFiles);
        var chatVm = DataContext as ChatViewModel;

        if (files.Count > 0)
        {
            chatVm!.AddSelectedFiles(files);
        }
    }

    private void TextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyModifiers == KeyModifiers.Shift && e.Key == Key.Enter)
        {
            InputMessageBox.Text += "\n";
            InputMessageBox.CaretIndex = InputMessageBox.Text!.Length;
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            var viewModel = (ChatViewModel)DataContext!;
            if (viewModel.SendMessageCommand.CanExecute(null))
            {
                viewModel.SendMessageCommand.Execute(null);
            }
            e.Handled = true;
        }
    }
}