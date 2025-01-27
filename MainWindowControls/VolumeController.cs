using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Xtrack.MainWindowControls
{
    public class VolumeController : IVolumeController
    {
        private readonly Grid _channelsGrid;
        private readonly MainWindowController _controller;

        public VolumeController(Grid channelsGrid, MainWindowController controller)
        {
            _channelsGrid = channelsGrid;
            _controller = controller;
        }

        public void HandleVolumeChanged(Slider slider)
        {
            if (TryGetChannelIndex(slider, out int channelIndex))
            {
                if (IsChannelMuted(channelIndex))
                {
                    Console.WriteLine($"Volume change ignored for muted Channel {channelIndex}");
                    return;
                }

                float volume = (float)slider.Value;
                Console.WriteLine($"Volume changed for Channel {channelIndex}. Volume: {volume}");
                _controller.SetChannelVolume(channelIndex, volume);
            }
            else
            {
                Console.WriteLine("Invalid or missing Tag in Slider.");
            }
        }

        public void HandleMuteToggled(CheckBox checkBox)
        {
            if (TryGetChannelIndex(checkBox, out int channelIndex))
            {
                bool isMuted = !checkBox.IsChecked ?? true;
                Console.WriteLine($"Mute changed for Channel {channelIndex}. IsMuted: {isMuted}");

                if (isMuted)
                {
                    _controller.SetChannelVolume(channelIndex, 0.0f);
                }
                else
                {
                    var slider = FindChildOfType<Slider>(_channelsGrid, s => TryGetChannelIndex(s, out int tagIndex) && tagIndex == channelIndex);
                    if (slider != null)
                    {
                        float volume = (float)slider.Value;
                        Console.WriteLine($"Unmuting Channel {channelIndex}, setting volume to {volume}");
                        _controller.SetChannelVolume(channelIndex, volume);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid or missing Tag in CheckBox.");
            }
        }

        public bool IsChannelMuted(int channelIndex)
        {
            var checkBox = FindChildOfType<CheckBox>(_channelsGrid, cb => cb.Tag?.ToString() == channelIndex.ToString());

            if (checkBox != null)
            {
                return checkBox.IsChecked == false;
            }

            Console.WriteLine($"Mute CheckBox not found for Channel {channelIndex}");
            return false;
        }

        public void ResetVolumeAndMute(Grid channelsGrid)
        {
            ResetChildControlsRecursive(channelsGrid);
        }

        private void ResetChildControlsRecursive(DependencyObject parent)
        {
            if (parent == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Slider slider)
                {
                    slider.Value = 0.5;
                }
                else if (child is CheckBox checkBox)
                {
                    checkBox.IsChecked = true; 
                }
                else
                {
                    ResetChildControlsRecursive(child);
                }
            }
        }

        private bool TryGetChannelIndex(FrameworkElement element, out int channelIndex)
        {
            if (element?.Tag != null && int.TryParse(element.Tag.ToString(), out channelIndex))
            {
                return true;
            }

            channelIndex = -1;
            return false;
        }

        private T? FindChildOfType<T>(DependencyObject parent, Func<T, bool>? condition = null) where T : DependencyObject
        {
            if (parent == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T typedChild && (condition == null || condition(typedChild)))
                {
                    return typedChild;
                }

                var result = FindChildOfType<T>(child, condition);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
