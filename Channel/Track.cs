using System;
using Common;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace Xtrack.Channel
{
    public class Track : ITrack
    {
        private string _filePath;

        private IChannelEqualizer? _channelEqualizer;
        private IWavePlayer? _wavePlayer;
        private EqualizerSampleProvider? _equalizerSampleProvider;
        private AudioFileReader? _audioFileReader;

        private float _volume = 0.5f;

        private bool _isWavePlayerInitialized;

        public Track(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            _filePath = filePath;

            _isWavePlayerInitialized = false;
        }

        public void SetFilepath(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));
            }

            _filePath = filePath;
        }

        public string GetFilepath()
        {
            return _filePath;
        }

        public void SetChannelEqualizer(IChannelEqualizer channelEqualizer)
        {
            _channelEqualizer = channelEqualizer;
        }

        public bool Play()
        {
            if (_channelEqualizer == null)
            {
                ErrorLogger.LogError("Cannot play: ChannelEqualizer is not set.");
                return false;
            }

            if (!playTrack())
            {
                ErrorLogger.LogError($"Cannot play {_filePath}");
                return false;
            }

            return true;
        }

        private bool playTrack()
        {
            if (!setWavePlayerAndSampleProvider())
            {
                resetWavePlayerAndSampleProvider();
                return false;
            }

            _wavePlayer.Play();
            return true;
        }

        private bool setWavePlayerAndSampleProvider()
        {
            try
            {

                if (_wavePlayer == null)
                {
                    _wavePlayer = new WaveOutEvent();
                    _isWavePlayerInitialized = false; 
                }

                if (_audioFileReader == null || _audioFileReader.FileName != _filePath)
                {
                    _audioFileReader?.Dispose(); 
                    _audioFileReader = new AudioFileReader(_filePath);
                }

                if (_equalizerSampleProvider == null)
                {
                    _equalizerSampleProvider = new EqualizerSampleProvider(_audioFileReader, _channelEqualizer!.GetBands());
                }
                else
                {
                    _equalizerSampleProvider.UpdateBands(_channelEqualizer.GetBands());
                }

                if (!_isWavePlayerInitialized)
                {
                    _wavePlayer.Init(_equalizerSampleProvider);
                    _isWavePlayerInitialized = true; 
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while setting WavePlayer and SampleProvider: {ex.Message}");
                return false;
            }
        }

        private void resetWavePlayerAndSampleProvider()
        {
            _wavePlayer?.Stop();
            _wavePlayer?.Dispose();
            _wavePlayer = null;

            _equalizerSampleProvider = null;

            _audioFileReader?.Dispose();
            _audioFileReader = null;
        }

        public bool Play(TimeSpan time)
        {
            if (!isValidPlaybackPosition(time))
            {
                ErrorLogger.LogError("Cannot play from a specific time: Invalid position.");
                return false;
            }

            if (!playTrackFromTime(time))
            {
                ErrorLogger.LogError($"Cannot play {_filePath} from time {time}.");
                return false;
            }

            return true;
        }

        private bool playTrackFromTime(TimeSpan time)
        {
            try
            {
                stopWavePlayer();

                if (!setWavePlayerAndSampleProvider())
                {
                    resetWavePlayerAndSampleProvider();
                    return false;
                }

                if (!seekToTime(time))
                {
                    return false;
                }

                _wavePlayer.Play();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while playing from specific time: {ex.Message}");
                return false;
            }
        }

        private bool seekToTime(TimeSpan time)
        {
            try
            {
                long bytePosition = (long)(_audioFileReader!.WaveFormat.AverageBytesPerSecond * time.TotalSeconds);

                if (bytePosition < 0 || bytePosition > _audioFileReader.Length)
                {
                    ErrorLogger.LogError("Seek position is out of range.");
                    return false;
                }

                _audioFileReader.Position = bytePosition;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while seeking to specific time: {ex.Message}");
                return false;
            }
        }

        private bool isValidPlaybackPosition(TimeSpan time)
        {
            if (_audioFileReader == null)
            {
                ErrorLogger.LogError("Cannot play from a specific time: Track is not initialized.");
                return false;
            }

            if (time < TimeSpan.Zero || time > _audioFileReader.TotalTime)
            {
                ErrorLogger.LogError("Cannot play from a specific time: Time is out of range.");
                return false;
            }

            return true;
        }

        public bool Stop()
        {
            if (_channelEqualizer == null)
            {
                ErrorLogger.LogError("Cannot stop: ChannelEqualizer is not set.");
                return false;
            }

            stopWavePlayer();
            return true;
        }

        private void stopWavePlayer()
        {
            if (_wavePlayer != null)
            {
                _wavePlayer.Stop();
                _wavePlayer.Dispose();
                _wavePlayer = null;

                Console.WriteLine($"Track is stopped: {_filePath}");
            }
        }

        public void UpdateEqualizer()
        {
            if (_equalizerSampleProvider != null && _channelEqualizer != null)
            {
                _equalizerSampleProvider.UpdateBands(_channelEqualizer.GetBands());
                Console.WriteLine("Equalizer updated successfully.");
            }
        }

        public TimeSpan GetLength()
        {
            if (_audioFileReader == null)
            {
                using (var reader = new AudioFileReader(_filePath))
                {
                    return reader.TotalTime;
                }
            }

            return _audioFileReader.TotalTime;
        }

        public void SetVolume(float volume)
        {
            if (_wavePlayer != null && _audioFileReader != null)
            {
                _volume = Math.Clamp(volume, 0.0f, 1.0f);
                _audioFileReader.Volume = _volume;
            }
        }

        public float GetVolume()
        {
            return _volume;
        }

        public ISampleProvider GetSampleProvider()
        {
            var audioFileReader = new AudioFileReader(_filePath);

            var volumeProvider = new VolumeSampleProvider(audioFileReader.ToSampleProvider())
            {
                Volume = _volume 
            };

            return ApplyEqualizer(volumeProvider);
        }

        private ISampleProvider ApplyEqualizer(ISampleProvider input)
        {
            if (_channelEqualizer == null)
            {
                throw new InvalidOperationException("Equalizer is not set for the channel.");
            }

            var bands = _channelEqualizer.GetBands();
            return new EqualizerSampleProvider(input, bands);
        }

    }
}
