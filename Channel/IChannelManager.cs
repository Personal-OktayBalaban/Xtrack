using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrack.Channel
{
    public interface IChannelManager
    {
        bool LoadChannels(string folderPath);
        IEnumerable<IChannel> GetChannels();
        void ClearChannels();

        bool PlayChannel(int index);
        bool PlayChannel(int index, TimeSpan time);
        bool StopChannel(int index);
        bool PlayAllChannels();
        bool PlayAllChannels(TimeSpan time);
        bool StopAllChannels();

        TimeSpan GetTrackLength();

        public void SetChannelVolume(int index, float volume);
        public void SetChannelMute(int index);

        EqualizerBand[] GetEqualizerBands(int channelIndex);
        bool UpdateEqualizerBand(int channelIndex, float frequency, float gain);
    }
}
