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
using System.Linq;
using System.Threading.Tasks;

namespace ClientApp.Views;

/// <summary>
/// Представляет элемент управления для отображения чата.
/// </summary>
public partial class ChatView : UserControl
{
    /// <summary>
    /// Провайдер хранилища.
    /// </summary>
    private readonly IStorageProvider _storageProvider;

    /// <summary>
    /// Входной поток аудио.
    /// </summary>

    private IWaveIn _input = null!;

    /// <summary>
    /// Буфер для записи аудио.
    /// </summary>
    private readonly List<byte> _buffer = [];

    /// <summary>
    /// Флаг, указывающий, происходит ли запись аудио.
    /// </summary>
    private bool _isRecord = false;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ChatView"/>.
    /// </summary>
    public ChatView()
    {
        _storageProvider = App
            .ServiceProvider
            .GetRequiredService<IStorageProvider>();

        InitializeComponent();
        AudioButton.Content = "▶";
        AddButton.Click += AddFile_Click;
    }

    /// <summary>
    /// Обрабатывает событие нажатия кнопки добавления файла.
    /// </summary>
    /// <param name="sender">Объект, вызвавший событие.</param>
    /// <param name="e">Аргументы события.</param>
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

    /// <summary>
    /// Обрабатывает событие нажатия клавиши в текстовом поле.
    /// </summary>
    /// <param name="sender">Объект, вызвавший событие.</param>
    /// <param name="e">Аргументы события.</param>
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
                viewModel
                    .SendMessageCommand
                    .Execute(null);
            }
            e.Handled = true;
        }
    }

    /// <summary>
    /// Обрабатывает событие нажатия кнопки аудио.
    /// </summary>
    /// <param name="sender">Объект, вызвавший событие.</param>
    /// <param name="e">Аргументы события.</param>
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

    /// <summary>
    /// Начинает запись аудио.
    /// </summary>
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

    /// <summary>
    /// Останавливает запись аудио и отправляет записанный файл.
    /// </summary>
    private async Task StopRecordAudio()
    {
        AudioButton.Content = "▶";
        _input.StopRecording();
        _input.Dispose();
        var vm = (ChatViewModel)DataContext!;
        await vm.SendAudioAsync(_buffer);
        _isRecord = false;
    }

    /// <summary>
    /// Обрабатывает событие загрузки элемента управления воспроизведения аудио.
    /// </summary>
    /// <param name="sender">Объект, вызвавший событие.</param>
    /// <param name="e">Аргументы события.</param>
    private void AudioPlayerControl_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is AudioPlayerControl audioPlayerControl)
        {
            var audioMessage = (AudioMessage)((ContentPresenter)audioPlayerControl.Parent!).Content!;
            audioPlayerControl.LoadAudio(audioMessage.UserName, [.. audioMessage.Audio]);
        }
    }
}