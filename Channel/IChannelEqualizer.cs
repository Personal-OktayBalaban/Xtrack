using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrack.Channel
{
    public interface IChannelEqualizer
    {
        EqualizerBand[] GetBands();

        bool UpdateBand(float frequency, float gain);

        void SetTrack(ITrack track);

    }
}
