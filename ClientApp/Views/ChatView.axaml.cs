using Avalonia.Controls;
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
            FileTypeFilter = [FilePickerFileTypes.ImageAll, FilePickerFileTypes.All]
        };

        var files = await _storageProvider.OpenFilePickerAsync(optionAllFiles);
        var chatVm = DataContext as ChatViewModel;

        if (files.Count > 0)
        {
            await chatVm!.AddFile(files);
        }
    }
}