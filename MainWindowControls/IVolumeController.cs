using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xtrack.MainWindowControls
{
    public interface IVolumeController
    {
        void HandleVolumeChanged(Slider slider);
        void HandleMuteToggled(CheckBox checkBox);
        bool IsChannelMuted(int channelIndex);
        void ResetVolumeAndMute(Grid channelsGrid);
    }
}
