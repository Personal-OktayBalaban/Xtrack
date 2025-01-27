using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Xtrack.MainWindowControls.EqualizerControllers
{
    internal class EqualizerChangeHandler
    {
        public void Handle(Slider slider)
        {
            if (!IsValidChannelTag(slider.Tag, out int channelIndex))
            {
                return;
            }

            float gain = (float)slider.Value;
            string frequencyName = slider.ToolTip?.ToString();

            float frequency = MapFrequencyNameToValue(frequencyName);

            AppManager.Instance.GetChannelManager().UpdateEqualizerBand(channelIndex, frequency, gain);
        }

        private bool IsValidChannelTag(object tag, out int channelIndex)
        {
            if (tag is string tagString && int.TryParse(tagString, out channelIndex))
            {
                return true;
            }

            channelIndex = -1;
            return false;
        }

        private float MapFrequencyNameToValue(string frequencyName)
        {
            return frequencyName switch
            {
                "Bass" => 100f,
                "Mid" => 1000f,
                "Treble" => 5000f,
                _ => throw new InvalidOperationException("Unknown frequency")
            };
        }
    }
}

