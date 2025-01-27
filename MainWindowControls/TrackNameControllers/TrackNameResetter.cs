using System.Windows.Controls;

namespace Xtrack.MainWindowControls.TrackNameControllers
{
    internal class TrackNameResetter
    {
        public void Reset(Grid channelsGrid)
        {
            foreach (var channelGrid in GetChannelGrids(channelsGrid))
            {
                ResetChannelTrackNames(channelGrid);
            }
        }

        private IEnumerable<Grid> GetChannelGrids(Grid channelsGrid)
        {
            foreach (var child in channelsGrid.Children)
            {
                if (child is Border channelBorder && channelBorder.Child is Grid channelGrid)
                {
                    yield return channelGrid;
                }
            }
        }

        private void ResetChannelTrackNames(Grid channelGrid)
        {
            foreach (var textBlock in GetTrackNameTextBlocks(channelGrid))
            {
                ResetTrackName(textBlock);
            }
        }

        private IEnumerable<TextBlock> GetTrackNameTextBlocks(Grid channelGrid)
        {
            foreach (var element in channelGrid.Children)
            {
                if (element is TextBlock textBlock && textBlock.Name.StartsWith("TrackNameChannel"))
                {
                    yield return textBlock;
                }
            }
        }

        private void ResetTrackName(TextBlock textBlock)
        {
            textBlock.Text = "No Track Loaded";
        }
    }
}
