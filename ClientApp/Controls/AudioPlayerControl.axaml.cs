using Avalonia.Controls;
using Avalonia.Interactivity;
using NAudio.Wave;
using System.IO;

namespace Chat.ClientApp.Controls
{
    /// <summary>
    /// Класс AudioPlayerControl представляет собой пользовательский элемент управления для воспроизведения аудио.
    /// </summary>
    public partial class AudioPlayerControl : UserControl
    {
        /// <summary>
        /// Объект для воспроизведения аудио.
        /// </summary>
        private IWavePlayer _waveOut = new WaveOut();

        /// <summary>
        /// Объект для чтения аудиоданных.
        /// </summary>
        private RawSourceWaveStream _waveStream = null!;

        /// <summary>
        /// Объект для хранения аудиоданных.
        /// </summary>
        private MemoryStream _stream = null!;

        /// <summary>
        /// Флаг, указывающий, воспроизводится ли аудио.
        /// </summary>
        private bool _isPlaying;

        /// <summary>
        /// Конструктор класса AudioPlayerControl.
        /// </summary>
        public AudioPlayerControl()
        {
            InitializeComponent();
            _waveOut.PlaybackStopped += OnPlaybackStopped;
            _isPlaying = false;
        }

        /// <summary>
        /// Загружает аудиоданные для воспроизведения.
        /// </summary>
        /// <param name="userName">Имя пользователя.</param>
        /// <param name="audioData">Аудиоданные.</param>
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

        /// <summary>
        /// Обработчик события нажатия кнопки "Play".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying) return;

            // Если аудио достигло конца, перезапускаем его
            if (_waveStream.Position == _waveStream.Length)
            {
                _stream.Position = 0;
                _waveStream.Position = 0;
            }

            _waveOut.Play();
            _isPlaying = true;
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Pause".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _waveOut.Pause();
                _isPlaying = false;
            }
        }

        /// <summary>
        /// Обработчик события нажатия кнопки "Stop".
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                _waveOut.Stop();
                _isPlaying = false;
            }
        }

        /// <summary>
        /// Обработчик события остановки воспроизведения аудио.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
        private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
        {
            _isPlaying = false;
        }

        /// <summary>
        /// Обработчик события выгрузки элемента управления.
        /// </summary>
        /// <param name="e">Аргументы события.</param>
        protected override void OnUnloaded(RoutedEventArgs e)
        {
            _waveOut?.Stop();
            _waveOut?.Dispose();
            _waveStream?.Dispose();
            _stream?.Dispose();
            base.OnUnloaded(e);
        }

        /// <summary>
        /// Обработчик события изменения значения ползунка громкости.
        /// </summary>
        /// <param name="sender">Объект, вызвавший событие.</param>
        /// <param name="e">Аргументы события.</param>
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