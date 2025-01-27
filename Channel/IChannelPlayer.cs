using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrack.Channel
{
    public interface IChannelPlayer
    {
        bool PlayChannel(List<IChannel> channels, int index);
        bool PlayChannel(List<IChannel> channels, int index, TimeSpan time);
        bool StopChannel(List<IChannel> channels, int index);
        bool PlayAllChannels(List<IChannel> channels);
        bool PlayAllChannels(List<IChannel> channels, TimeSpan time);
        bool StopAllChannels(List<IChannel> channels);
    }
}
