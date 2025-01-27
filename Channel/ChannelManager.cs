using System;
using System.Collections.Generic;
using Common;

namespace Xtrack.Channel
{
    public class ChannelManager : IChannelManager
    {
        private readonly List<IChannel> _channels;
        private readonly IChannelLoader _channelLoader;
        private readonly IChannelPlayer _channelPlayer;

        private TimeSpan _trackDuration;

        public ChannelManager()
        {
            _channels = new List<IChannel>();
            _channelLoader = new ChannelLoader();
            _channelPlayer = new ChannelPlayer();
        }

        public bool LoadChannels(string folderPath)
        {
            try
            {
                ClearChannels();

                var loadedChannels = _channelLoader.LoadChannels(folderPath);
                _channels.Clear();
                _channels.AddRange(loadedChannels);

                _trackDuration = _channels.First().GetTrackLength();

                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to load channels: {ex.Message}");
                return false;
            }
        }

        public void ClearChannels()
        {
            foreach (var channel in _channels)
            {
                channel.Stop(); 
            }
            _channels.Clear();
            _trackDuration = TimeSpan.Zero;
        }

        public IEnumerable<IChannel> GetChannels()
        {
            return _channels;
        }

        public bool PlayChannel(int index)
        {
            try
            {
                return _channelPlayer.PlayChannel(_channels, index);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to play channel at index {index}: {ex.Message}");
                return false;
            }
        }

        public bool PlayChannel(int index, TimeSpan time)
        {
            try
            {
                return _channelPlayer.PlayChannel(_channels, index, time);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to play channel at index {index} from time {time}: {ex.Message}");
                return false;
            }
        }

        public bool StopChannel(int index)
        {
            try
            {
                return _channelPlayer.StopChannel(_channels, index);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to stop channel at index {index}: {ex.Message}");
                return false;
            }
        }

        public bool PlayAllChannels()
        {
            try
            {
                foreach (var channel in _channels)
                {
                    ApplyChannelSettings(channel);

                    channel.Play();
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to play all channels: {ex.Message}");
                return false;
            }
        }

        public bool PlayAllChannels(TimeSpan time)
        {
            try
            {
                foreach (var channel in _channels)
                {
                    ApplyChannelSettings(channel);

                    channel.Play(time);
                }
                return true;
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to play all channels from time {time}: {ex.Message}");
                return false;
            }
        }

        private void ApplyChannelSettings(IChannel channel)
        {
            channel.SetVolume(channel.GetVolume());

            if (channel.IsMuted())
            {
                channel.Mute();
            }
            else
            {
                channel.Unmute();
            }

            // Apply equalizer settings.
            var equalizer = channel.GetEqualizer();
            foreach (var band in equalizer.GetBands())
            {
                equalizer.UpdateBand(band.Frequency, band.Gain);
            }
        }

        public bool StopAllChannels()
        {
            try
            {
                return _channelPlayer.StopAllChannels(_channels);
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError($"Failed to stop all channels: {ex.Message}");
                return false;
            }
        }

        public TimeSpan GetTrackLength()
        {
            return _trackDuration;
        }


        public void SetChannelVolume(int index, float volume)
        {
            if (index >= 0 && index < _channels.Count)
            {
                _channels[index].SetVolume(volume);
            }
        }

        public void SetChannelMute(int index)
        {
            if (index >= 0 && index < _channels.Count)
            {
                _channels[index].Mute();
            }
        }

        public EqualizerBand[] GetEqualizerBands(int channelIndex)
        {
            if (channelIndex >= 0 && channelIndex < _channels.Count)
            {
                return _channels[channelIndex].GetEqualizer().GetBands();
            }
            return Array.Empty<EqualizerBand>();
        }

        public bool UpdateEqualizerBand(int channelIndex, float frequency, float gain)
        {
            if (channelIndex >= 0 && channelIndex < _channels.Count)
            {
                return _channels[channelIndex].GetEqualizer().UpdateBand(frequency, gain);
            }
            return false;
        }
    }
}
