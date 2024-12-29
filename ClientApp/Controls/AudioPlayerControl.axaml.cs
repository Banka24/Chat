using Avalonia.Controls;
using Avalonia.Interactivity;
using NAudio.Wave;
using System.IO;

namespace Chat.ClientApp.Controls
{
    public partial class AudioPlayerControl : UserControl
    {
        private IWavePlayer _waveOut = new WaveOut();
        private RawSourceWaveStream _waveStream = null!;
        private MemoryStream _stream = null!;
        private bool _isPlaying;

        public AudioPlayerControl()
        {
            InitializeComponent();
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _isPlaying = false;
        }

        public void LoadAudio(string userName, byte[] audioData)
        {
            UserNameText.Text = userName;

            if (_waveStream != null)
            {
                _waveStream.Dispose();
                _waveStream = null!;
            }

            if (_stream != null)
            {
                _stream.Dispose();
                _stream = null!;
            }

            _stream = new MemoryStream(audioData);
            _waveStream = new RawSourceWaveStream(_stream, new WaveFormat(44100, 24, 2));
            _waveOut.Init(_waveStream);
            _waveOut.Volume = 1.0f;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying) return;

            // ѕроверка состо€ни€ перед воспроизведением
            if (_waveStream.Position == _waveStream.Length)
            {
                _stream.Position = 0;
                _waveStream.Position = 0;
            }

            _waveOut.Play();
            _isPlaying = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _waveOut.Pause();
                _isPlaying = false;
            }
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _waveOut.Stop();
                _isPlaying = false;
            }
        }

        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
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
                VolumeValueText.Text = ((int)(e.NewValue * 100)).ToString();
                _waveOut.Volume = (float)e.NewValue;
            }
        }
    }
}