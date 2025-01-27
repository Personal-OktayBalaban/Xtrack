using System.Windows.Controls;
using Xtrack.Channel;

namespace Xtrack.MainWindowControls
{
    public interface ITrackNameController
    {
        void ResetTrackNames(Grid channelsGrid);
        void UpdateTrackNames(Grid channelsGrid, IEnumerable<IChannel> channels);
    }
}
