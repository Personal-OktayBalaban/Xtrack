using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xtrack.Channel;

namespace Xtrack
{
    public class AppManager
    {
        private static AppManager _instance;

        private IChannelManager _channelManager;

        public static AppManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppManager();
                }
                return _instance;
            }
        }

        private AppManager()
        {
            _channelManager = new ChannelManager();
        }

        public IChannelManager GetChannelManager()
        {
            return _channelManager;
        }
            
    }
}
