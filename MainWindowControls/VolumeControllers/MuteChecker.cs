using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xtrack.MainWindowControls.VolumeControllers
{
    internal class MuteChecker
    {
        private readonly Grid _channelsGrid;

        public MuteChecker(Grid channelsGrid)
        {
            _channelsGrid = channelsGrid;
        }

        public bool IsMuted(int channelIndex)
        {
            var checkBox = FindCheckBoxForChannel(channelIndex);
            return IsCheckBoxMuted(checkBox, channelIndex);
        }

        private CheckBox? FindCheckBoxForChannel(int channelIndex)
        {
            return FindChildOfType<CheckBox>(_channelsGrid, cb => IsMatchingChannel(cb, channelIndex));
        }

        private bool IsMatchingChannel(CheckBox checkBox, int channelIndex)
        {
            return checkBox.Tag?.ToString() == channelIndex.ToString();
        }

        private bool IsCheckBoxMuted(CheckBox? checkBox, int channelIndex)
        {
            if (checkBox == null)
            {
                LogMissingCheckBox(channelIndex);

                return false;
            }

            return checkBox.IsChecked == false;
        }

        private void LogMissingCheckBox(int channelIndex)
        {
            Console.WriteLine($"Mute CheckBox not found for Channel {channelIndex}");
        }

        private T? FindChildOfType<T>(DependencyObject parent, Func<T, bool>? condition = null) where T : DependencyObject
        {
            foreach (var child in GetChildren(parent))
            {
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