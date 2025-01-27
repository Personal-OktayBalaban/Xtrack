using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xtrack.Channel
{
    public interface IChannelLoader
    {
        List<IChannel> LoadChannels(string folderPath);
    }
}
