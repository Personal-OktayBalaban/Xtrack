using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Xtrack.Channel;

namespace Xtrack.MainWindowControls.EqualizerControllers
{
    internal class EqualizerInitializer
    {
        public void Initialize(Grid channelsGrid, MainWindowController controller)
        {
            for (int channelIndex = 0; channelIndex < channelsGrid.Children.Count; channelIndex++)
            {
                InitializeChannel(channelsGrid, controller, channelIndex);
            }
        }

        private void InitializeChannel(Grid channelsGrid, MainWindowController controller, int channelIndex)
        {
            var bands = controller.GetEqualizerBands(channelIndex);

            foreach (var band in bands)
            {
                InitializeSlider(channelsGrid, channelIndex, band);
            }
        }

        private void InitializeSlider(Grid channelsGrid, int channelIndex, EqualizerBand band)
        {
            var slider = FindSlider(channelsGrid, channelIndex, band.Frequency);

            if (slider != null)
            {
                slider.Value = band.Gain;
            }
        }

        private Slider? FindSlider(Grid channelsGrid, int channelIndex, float frequency)
        {
            foreach (var child in channelsGrid.Children)
            {
                if (IsChannelGridWithMatchingSlider(child, channelIndex, frequency, out Slider? slider))
                {
                    return slider;
                }
            }
            return null;
        }

        private bool IsChannelGridWithMatchingSlider(object child, int channelIndex, float frequency, out Slider? slider)
        {
            if (child is Grid channelGrid)
            {
                return TryFindSliderInChannel(channelGrid, channelIndex, frequency, out slider);
            }

            slider = null;
            return false;
        }

        private bool TryFindSliderInChannel(Grid channelGrid, int channelIndex, float frequency, out Slider? slider)
        {
            foreach (var gridChild in channelGrid.Children)
            {
                if (IsSliderMatching(gridChild, channelIndex, frequency, out slider))
                {
                    return true;
                }
            }

            slider = null;
            return false;
        }

        private bool IsSliderMatching(object gridChild, int channelIndex, float frequency, out Slider? slider)
        {
            if (gridChild is Slider potentialSlider && IsMatchingSlider(potentialSlider, channelIndex, frequency))
            {
                slider = potentialSlider;
                return true;
            }

            slider = null;
            return false;
        }

        private bool IsMatchingSlider(Slider slider, int channelIndex, float frequency)
        {
            if (slider.Tag?.ToString() != channelIndex.ToString())
            {
                return false;
            }

            string tooltip = slider.ToolTip?.ToString();

            if (tooltip == "Bass" && frequency == 100f)
            {
                return true;
            }
            if (tooltip == "Mid" && frequency == 1000f)
            {
                return true;
            }

            if (tooltip == "Treble" && frequency == 5000f)
            {
                return true;
            }

            return false;
        }
    }
}
