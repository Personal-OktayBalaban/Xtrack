using System.Collections.Generic;
using Xtrack.Channel;

namespace Xtrack.Saving
{
    public interface ITrackMerger
    {
        bool MergeAndSave(IEnumerable<IChannel> channels, string outputPath);
    }
}
