using System;
using System.Windows.Controls;
using System.Windows;

namespace Xtrack.MainWindowControls.VolumeControllers
{
    internal class VolumeChangeHandler
    {
        private readonly Grid _channelsGrid;
        private readonly MainWindowController _controller;

        public VolumeChangeHandler(Grid channelsGrid, MainWindowController controller)
        {
            _channelsGrid = channelsGrid;
            _controller = controller;
        }

        public void Handle(Slider slider)
        {
            if (!TryGetChannelIndex(slider, out int channelIndex))
            {
                return;
            }

            if (IsChannelMuted(channelIndex))
            {
                return;
            }

            float volume = GetSliderValue(slider);
            SetChannelVolume(channelIndex, volume);
        }

        private bool IsChannelMuted(int channelIndex)
        {
            return new MuteChecker(_channelsGrid).IsMuted(channelIndex);
        }

        private float GetSliderValue(Slider slider)
        {
            return (float)slider.Value;
        }

        private void SetChannelVolume(int channelIndex, float volume)
        {
            _controller.SetChannelVolume(channelIndex, volume);
        }

        private bool TryGetChannelIndex(FrameworkElement element, out int channelIndex)
        {
            if (element?.Tag != null && int.TryParse(element.Tag.ToString(), out channelIndex))
            {
                return true;
            }

            channelIndex = -1;
            return false;
        }
    }
}