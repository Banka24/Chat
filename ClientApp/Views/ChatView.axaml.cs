using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Chat.ClientApp.Controls;
using Chat.ClientApp.DTO;
using ClientApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using NAudio.Wave;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Views;

public partial class ChatView : UserControl
{
    private readonly IStorageProvider _storageProvider;
    private WaveIn _input = null!;
    private readonly List<byte> _buffer = [];
    private bool _isRecord = false;

    public ChatView()
    {
        _storageProvider = App.ServiceProvider.GetRequiredService<IStorageProvider>();
        InitializeComponent();
        AudioButton.Content = "|>";
        AddButton.Click += AddFile_Click;
    }

    private async void AddFile_Click(object? sender, RoutedEventArgs e)
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

    private void AudioClick(object? sender, RoutedEventArgs e)
    {
        if (!_isRecord)
        {
            StartRecordAudio();
        }
        else
        {
            _ = StopRecordAudio();
        }
    }

    private void StartRecordAudio()
    {
        AudioButton.Content = "■";
        _buffer.Clear();
        _isRecord = true;

        _input = new WaveIn()
        {
            WaveFormat = new WaveFormat(44100, 24, 2)
        };

        _input.DataAvailable += (sender, e) =>
        {
            _buffer.AddRange(e.Buffer.Take(e.BytesRecorded));
        };

        _input.StartRecording();
    }

    private async Task StopRecordAudio()
    {
        AudioButton.Content = "▶";
        _input.StopRecording();
        _input.Dispose();
        var vm = (ChatViewModel)DataContext!;
        await vm.SendAudioAsync(_buffer);
        _isRecord = false;
    }

    private void AudioPlayerControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is AudioPlayerControl audioPlayerControl)
        {
            var audioMessage = (AudioMessage)((ContentPresenter)audioPlayerControl.Parent!).Content!;
            audioPlayerControl.LoadAudio(audioMessage.UserName, audioMessage.Audio.ToArray());
        }
    }
}