using System;
using System.Windows;
using Xtrack.Channel;

namespace Xtrack.MainWindowControls
{
    public class MainWindowController
    {
        private readonly IChannelManager _channelManager;

        private bool _areChannelsPlaying; 
        private TimeSpan _currentPlaybackTime;



        public MainWindowController()
        {
            _channelManager = AppManager.Instance.GetChannelManager();

            _areChannelsPlaying = false;
            _currentPlaybackTime = TimeSpan.Zero; 
        }

        public void LoadFolderAndConfigureChannels()
        {
            var folderPath = selectFolder();

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                MessageBox.Show("No folder selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!_channelManager.LoadChannels(folderPath))
            {
                MessageBox.Show("Failed to load channels.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _areChannelsPlaying = false;
            _currentPlaybackTime = TimeSpan.Zero;
            MessageBox.Show("Channels loaded successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public TimeSpan GetTrackLength()
        {
            return _channelManager.GetTrackLength();
        }

        public TimeSpan GetCurrentPlaybackTime()
        {
            return _currentPlaybackTime;
        }

        public void SetPlaybackTime(TimeSpan time)
        {
            _currentPlaybackTime = time; 

            if (_areChannelsPlaying)
            {
                PlayAllChannelsFrom(_currentPlaybackTime);
            }
        }

        public void PlayAllChannels()
        {
            if (!_areChannelsPlaying)
            {
                PlayAllChannelsFrom(_currentPlaybackTime);
                _areChannelsPlaying = true;
            }
        }

        public void StopAllChannels()
        {
            if (_areChannelsPlaying)
            {
                _channelManager.StopAllChannels();
                _areChannelsPlaying = false; 
            }
        }

        public void PlayAllChannelsFrom(TimeSpan time)
        {
            if (time == TimeSpan.Zero)
            {
                playFromBeginning();
            }
            else
            {
                playFromTimePoint(time);
            }
        }

        private void playFromBeginning()
        {
            if (!_channelManager.PlayAllChannels())
            {
                MessageBox.Show("Failed to play all channels from the beginning.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _areChannelsPlaying = true;
            }
        }

        private void playFromTimePoint(TimeSpan time)
        {
            if (!_channelManager.PlayAllChannels(time))
            {
                MessageBox.Show("Failed to play all channels from the specified time.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _areChannelsPlaying = true;
            }
        }

        public bool AreChannelsPlaying()
        {
            return _areChannelsPlaying;
        }

        public IEnumerable<IChannel> GetAllChannels()
        {
            return _channelManager.GetChannels();
        }

        private string selectFolder()
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            var result = dialog.ShowDialog();
            return result == System.Windows.Forms.DialogResult.OK ? dialog.SelectedPath : null;
        }

        public void SetChannelVolume(int index, float volume)
        {
            _channelManager.SetChannelVolume(index, volume);
        }

        public void SetChannelMute(int index)
        {
            _channelManager.SetChannelMute(index);
        }

        public void UpdateEqualizerBand(int channelIndex, float frequency, float gain)
        {
            if (!_channelManager.UpdateEqualizerBand(channelIndex, frequency, gain))
            {
                MessageBox.Show($"Failed to update equalizer for channel {channelIndex}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public EqualizerBand[] GetEqualizerBands(int channelIndex)
        {
            return _channelManager.GetEqualizerBands(channelIndex);
        }

        public int GetLoadedChannelCount()
        {
            return _channelManager.GetChannels().Count();
        }

        public void ClearChannels()
        {
            _channelManager.ClearChannels(); 
            _areChannelsPlaying = false; 
            _currentPlaybackTime = TimeSpan.Zero; 
        }

        public void UpdateCurrentPlaybackTime(TimeSpan currentTime)
        {
            _currentPlaybackTime = currentTime;
        }

    }
}
