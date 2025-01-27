using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

namespace Xtrack.MainWindowControls.VolumeControllers
{
    public class VolumeController : IVolumeController
    {
        private readonly VolumeChangeHandler _volumeChangeHandler;
        private readonly MuteToggleHandler _muteToggleHandler;
        private readonly MuteChecker _muteChecker;
        private readonly VolumeAndMuteResetter _volumeAndMuteResetter;

        public VolumeController(Grid channelsGrid, MainWindowController controller)
        {
            _volumeChangeHandler = new VolumeChangeHandler(channelsGrid, controller);
            _muteToggleHandler = new MuteToggleHandler(channelsGrid, controller);
            _muteChecker = new MuteChecker(channelsGrid);
            _volumeAndMuteResetter = new VolumeAndMuteResetter();
        }

        public void HandleVolumeChanged(Slider slider)
        {
            _volumeChangeHandler.Handle(slider);
        }

        public void HandleMuteToggled(CheckBox checkBox)
        {
            _muteToggleHandler.Handle(checkBox);
        }

        public bool IsChannelMuted(int channelIndex)
        {
            return _muteChecker.IsMuted(channelIndex);
        }

        public void ResetVolumeAndMute(Grid channelsGrid)
        {
            _volumeAndMuteResetter.Reset(channelsGrid);
        }
    }
}
