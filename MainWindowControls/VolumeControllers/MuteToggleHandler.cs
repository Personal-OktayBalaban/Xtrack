using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Xtrack.MainWindowControls.VolumeControllers
{
    internal class MuteToggleHandler
    {
        private readonly Grid _channelsGrid;
        private readonly MainWindowController _controller;

        public MuteToggleHandler(Grid channelsGrid, MainWindowController controller)
        {
            _channelsGrid = channelsGrid;
            _controller = controller;
        }

        public void Handle(CheckBox checkBox)
        {
            if (!TryGetChannelIndex(checkBox, out int channelIndex))
            {
                LogInvalidCheckBox();
                return;
            }

            bool isMuted = IsCheckBoxMuted(checkBox);
            LogMuteStateChange(channelIndex, isMuted);

            if (isMuted)
            {
                MuteChannel(channelIndex);
            }
            else
            {
                UnmuteChannel(channelIndex);
            }
        }

        private void MuteChannel(int channelIndex)
        {
            _controller.SetChannelVolume(channelIndex, 0.0f);
        }

        private void UnmuteChannel(int channelIndex)
        {
            var slider = FindSliderForChannel(channelIndex);
            if (slider == null) return;

            float volume = GetSliderValue(slider);
            LogUnmuteAction(channelIndex, volume);
            _controller.SetChannelVolume(channelIndex, volume);
        }

        private Slider? FindSliderForChannel(int channelIndex)
        {
            return FindChildOfType<Slider>(_channelsGrid, s => TryGetChannelIndex(s, out int tagIndex) && tagIndex == channelIndex);
        }

        private float GetSliderValue(Slider slider)
        {
            return (float)slider.Value;
        }

        private bool IsCheckBoxMuted(CheckBox checkBox)
        {
            return !checkBox.IsChecked ?? true;
        }

        private void LogInvalidCheckBox()
        {
            Console.WriteLine("Invalid or missing Tag in CheckBox.");
        }

        private void LogMuteStateChange(int channelIndex, bool isMuted)
        {
            Console.WriteLine($"Mute changed for Channel {channelIndex}. IsMuted: {isMuted}");
        }

        private void LogUnmuteAction(int channelIndex, float volume)
        {
            Console.WriteLine($"Unmuting Channel {channelIndex}, setting volume to {volume}");
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
            foreach (var child in GetChildren(parent))
            {
                if (child is T typedChild && (condition?.Invoke(typedChild) ?? true))
                {
                    return typedChild;
                }

                if (FindChildOfType<T>(child, condition) is T result)
                {
                    return result;
                }
            }

            return null;
        }

        private IEnumerable<DependencyObject> GetChildren(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                yield return VisualTreeHelper.GetChild(parent, i);
            }
        }
    }
}
