using Avalonia.Controls;
using NAudio.Wave;
using System.IO;
using Avalonia.Interactivity;
using System.Threading.Tasks;

namespace Chat.ClientApp.Controls
{
    public partial class AudioPlayerControl : UserControl
    {
        private IWavePlayer _waveOut = new WaveOut();
        private RawSourceWaveStream _waveStream = null!;
        private TaskCompletionSource<bool> _tcs = null!;
        private MemoryStream _stream = null!;
        private bool _isPlaying = false;

        public AudioPlayerControl()
        {
            InitializeComponent();
            _waveOut.PlaybackStopped += OnPlaybackStopped; // ѕодписка на событие остановки воспроизведени€
        }

        public void LoadAudio(string userName, byte[] audioData)
        {
            UserNameText.Text = userName;
            _waveStream?.Dispose();
            _waveOut?.Stop();

            _stream = new MemoryStream(audioData);
            _waveStream = new RawSourceWaveStream(_stream, new WaveFormat(44100, 24, 2));
            _waveOut?.Init(_waveStream);
            _waveOut!.Volume = 1.0f;
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
                return;

            _tcs = new TaskCompletionSource<bool>();
            _waveOut.Play();
            _isPlaying = true;

            await _tcs.Task;

            _isPlaying = false;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _waveOut?.Pause();
                _tcs.SetResult(true);
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _waveOut?.Stop();
                _tcs.SetResult(true);
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            // «авершаем задачу, когда воспроизведение останавливаетс€
            if (_tcs != null && !_tcs.Task.IsCompleted)
            {
                _tcs.SetResult(true);
            }
            _isPlaying = false;
        }

        protected override void OnUnloaded(RoutedEventArgs e)
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveStream?.Dispose();
            _stream?.Dispose();
            base.OnUnloaded(e);
        }

        private void Slider_ValueChanged(object? sender, Avalonia.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            if (_waveOut != null)
            {
                _waveOut.Volume = (float)e.NewValue;
            }
        }
    }
}
