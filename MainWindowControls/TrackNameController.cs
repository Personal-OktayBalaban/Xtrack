using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xtrack.Channel;

namespace Xtrack.MainWindowControls
{
    public class TrackNameController : ITrackNameController
    {
        public void ResetTrackNames(Grid channelsGrid)
        {
            foreach (var child in channelsGrid.Children)
            {
                if (child is Border channelBorder && channelBorder.Child is Grid channelGrid)
                {
                    foreach (var element in channelGrid.Children)
                    {
                        if (element is TextBlock textBlock && textBlock.Name.StartsWith("TrackNameChannel"))
                        {
                            textBlock.Text = "No Track Loaded";
                        }
                    }
                }
            }
        }

        public void UpdateTrackNames(Grid channelsGrid, IEnumerable<IChannel> channels)
        {
            int index = 0;
            foreach (var channel in channels)
            {
                var textBlock = FindChild<TextBlock>(channelsGrid, $"TrackNameChannel{index + 1}");
                if (textBlock != null)
                {
                    textBlock.Text = channel.GetTrackName();
                }
                index++;
            }
        }

        private T? FindChild<T>(DependencyObject parent, string childName) where T : FrameworkElement
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is T element && element.Name == childName)
                {
                    return element;
                }

                var result = FindChild<T>(child, childName);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
    }
}
