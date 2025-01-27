using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Xtrack.Channel;
using Xtrack.MainWindowControls;
using Xtrack.MainWindowControls.EqualizerControllers;
using Xtrack.MainWindowControls.TrackNameControllers;
using Xtrack.MainWindowControls.VolumeControllers;
using Xtrack.TrackSaving;

namespace Xtrack
{
    public partial class MainWindow : Window
    {
        private readonly AppManager _appManager;
        private readonly MainWindowController _controller;

        private IPlaybackSliderUpdater _playbackSliderUpdater;
        private readonly IVolumeController _volumeController;
        private readonly IEqualizerController _equalizerController;
        private readonly ITrackNameController _trackNameController;

        private bool _isUserInteractingWithSlider;

        private TimeSpan _trackDuration;

        private readonly bool _isConstructed;


        public MainWindow()
        {
            _isConstructed = false;

            InitializeComponent();
            DisableAllControls();

            _appManager = AppManager.Instance;

            _controller = new MainWindowController();

            _volumeController = new VolumeController(ChannelsGrid, _controller);
            _equalizerController = new EqualizerController();
            _trackNameController = new TrackNameController();

            _isConstructed = true;
        }

        private void OnLoadFolderClick(object sender, RoutedEventArgs e)
        {
            DisableAllControls();

            _controller.ClearChannels();

            _controller.LoadFolderAndConfigureChannels();
            int channelCount = _controller.GetLoadedChannelCount();

            if (channelCount > 0)
            {
                ResetChannelUI();
                EnableControlsForLoadedChannels(channelCount);

                var channels = _controller.GetAllChannels();
                _trackNameController.UpdateTrackNames(ChannelsGrid, channels); 
            }
            else
            {
                MessageBox.Show("No valid channels found in the folder.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DisableAllControls();
            }
        }

        private void ResetChannelUI()
        {
            foreach (var child in ChannelsGrid.Children)
            {
                if (child is Border channelBorder)
                {
                    channelBorder.IsEnabled = false;
                }
            }

            _volumeController.ResetVolumeAndMute(ChannelsGrid);
            _equalizerController.ResetEqualizerControls(ChannelsGrid);
            _trackNameController.ResetTrackNames(ChannelsGrid);
            resetPlaybackSlider();
        }

        private void resetPlaybackSlider()
        {
            TotalTimeText.Text = "00:00";
            CurrentTimeText.Text = "00:00";
            PlaybackSlider.Value = 0;
            PlaybackSlider.IsEnabled = false;
        }


        private void DisableAllControls()
        {
            PlayAllButton.IsEnabled = false;
            StopAllButton.IsEnabled = false;
            SaveButton.IsEnabled = false;

            foreach (var child in ChannelsGrid.Children)
            {
                if (child is Border channelBorder)
                {
                    channelBorder.IsEnabled = false;
                }
            }

            TotalTimeText.Text = "00:00";
            CurrentTimeText.Text = "00:00";
            PlaybackSlider.IsEnabled = false;
        }

        private void EnableControlsForLoadedChannels(int channelCount)
        {
            PlayAllButton.IsEnabled = channelCount > 0;
            StopAllButton.IsEnabled = channelCount > 0;
            SaveButton.IsEnabled = channelCount > 0;

            for (int i = 0; i < ChannelsGrid.Children.Count; i++)
            {
                if (ChannelsGrid.Children[i] is Border channelBorder)
                {
                    channelBorder.IsEnabled = i < channelCount;
                }
            }

            if (channelCount > 0)
            {
                EnablePlaybackControls();
            }
            else
            {
                TotalTimeText.Text = "00:00";
                CurrentTimeText.Text = "00:00";
                PlaybackSlider.IsEnabled = false;
            }
        }

        private void EnablePlaybackControls()
        {
            _trackDuration = _controller.GetTrackLength();

            if (_trackDuration > TimeSpan.Zero)
            {
                TotalTimeText.Text = FormatTime(_trackDuration);
                CurrentTimeText.Text = FormatTime(TimeSpan.Zero);

                initializePlaybackSlider();
                initializeEqualizerControls();
            }
        }

        private void initializePlaybackSlider()
        {
            PlaybackSlider.IsEnabled = true;
            PlaybackSlider.Maximum = _trackDuration.TotalMilliseconds;

            _playbackSliderUpdater = new PlaybackSliderUpdater(
                _controller.GetTrackLength,
                OnUpdateSliderTime 
            );

            PlaybackSlider.PreviewMouseDown += OnPlaybackSliderMouseDown;
            PlaybackSlider.PreviewMouseUp += OnPlaybackSliderMouseReleased;
        }

        private void initializeEqualizerControls()
        {
            _equalizerController.InitializeEqualizerControls(ChannelsGrid, _controller);
        }

        private void OnPlayAllClick(object sender, RoutedEventArgs e)
        {
            _controller.PlayAllChannels();
            _playbackSliderUpdater?.Start(_controller.GetCurrentPlaybackTime());
        }

        private void OnStopAllClick(object sender, RoutedEventArgs e)
        {
            if (PlaybackSlider.IsEnabled)
            {
                var playbackTime = TimeSpan.FromMilliseconds(
                    _trackDuration.TotalMilliseconds * (PlaybackSlider.Value / PlaybackSlider.Maximum)
                );

                _controller.UpdateCurrentPlaybackTime(playbackTime);
            }

            _controller.StopAllChannels();
            _playbackSliderUpdater?.Stop();
            _isUserInteractingWithSlider = false;
        }


        private void OnPlaybackSliderMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isUserInteractingWithSlider = true;
            _playbackSliderUpdater?.Stop();
        }

        private void OnPlaybackSliderMouseReleased(object sender, MouseButtonEventArgs e)
        {
            if (PlaybackSlider.IsEnabled)
            {
                var playbackTime = TimeSpan.FromMilliseconds(
                    _trackDuration.TotalMilliseconds * (PlaybackSlider.Value / PlaybackSlider.Maximum)
                );

                _controller.SetPlaybackTime(playbackTime);
                _playbackSliderUpdater?.SetCurrentTime(playbackTime); 
                _playbackSliderUpdater?.Start(playbackTime); 

                CurrentTimeText.Text = FormatTime(playbackTime);
                _isUserInteractingWithSlider = false;
            }
        }

        private void OnUpdateSliderTime(TimeSpan currentTime, TimeSpan totalTime)
        {
            if (!_isUserInteractingWithSlider && _controller.AreChannelsPlaying())
            {
                PlaybackSlider.Value = currentTime.TotalMilliseconds;
                CurrentTimeText.Text = FormatTime(currentTime);
                TotalTimeText.Text = FormatTime(totalTime);
            }
        }

        private string FormatTime(TimeSpan time)
        {
            return time.ToString(@"mm\:ss");
        }

        private void OnVolumeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isConstructed)
            {
                return;
            }

            if (sender is Slider slider)
            {
                _volumeController.HandleVolumeChanged(slider);
            }
        }

        private void OnMuteToggleChange(object sender, RoutedEventArgs e)
        {
            if (!_isConstructed)
            {
                return;
            }

            if (sender is CheckBox checkBox)
            {
                _volumeController.HandleMuteToggled(checkBox);
            }
        }

        private void OnEqualizerChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (!_isConstructed)
            {
                return;
            }

            if (sender is Slider slider)
            {
                _equalizerController.HandleEqualizerChanged(slider);
            }
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "WAV Files (*.wav)|*.wav",
                Title = "Save Merged Track",
                DefaultExt = ".wav",
                FileName = "MergedTrack"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var outputPath = saveFileDialog.FileName;
                var channels = _controller.GetAllChannels();

                var trackMerger = new TrackMerger();
                if (trackMerger.MergeAndSave(channels, outputPath))
                {
                    MessageBox.Show($"Track saved successfully at {outputPath}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to save the track. Please try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

}
