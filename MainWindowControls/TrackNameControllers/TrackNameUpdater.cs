using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xtrack.Channel;

namespace Xtrack.MainWindowControls.TrackNameControllers
{
    internal class TrackNameUpdater
    {
        public void Update(Grid channelsGrid, IEnumerable<IChannel> channels)
        {
            int index = 0;
            
            foreach (var channel in channels)
            {
                UpdateChannelTrackName(channelsGrid, channel, index);
                index++;
            }
        }

        private void UpdateChannelTrackName(Grid channelsGrid, IChannel channel, int index)
        {
            var textBlock = FindTextBlockByName(channelsGrid, $"TrackNameChannel{index + 1}");
            
            if (textBlock != null)
            {
                SetTrackName(textBlock, channel.GetTrackName());
            }
        }

        private TextBlock? FindTextBlockByName(DependencyObject parent, string name)
        {
            foreach (var child in GetChildren(parent))
            {
                if (child is TextBlock textBlock && textBlock.Name == name)
                {
                    return textBlock;
                }

                var result = FindTextBlockByName(child, name);
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

        private void SetTrackName(TextBlock textBlock, string trackName)
        {
            textBlock.Text = trackName;
        }
    }
}