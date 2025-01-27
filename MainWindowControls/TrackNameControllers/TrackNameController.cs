using System.Windows.Controls;
using Xtrack.Channel;
using Xtrack.MainWindowControls.TrackNameControllers;

public class TrackNameController : ITrackNameController
{
    private readonly TrackNameResetter _resetter;
    private readonly TrackNameUpdater _updater;

    public TrackNameController()
    {
        _resetter = new TrackNameResetter();
        _updater = new TrackNameUpdater();
    }

    public void ResetTrackNames(Grid channelsGrid)
    {
        _resetter.Reset(channelsGrid);
    }

    public void UpdateTrackNames(Grid channelsGrid, IEnumerable<IChannel> channels)
    {
        _updater.Update(channelsGrid, channels);
    }
}