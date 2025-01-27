using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;
using System.Windows;

namespace Xtrack.MainWindowControls
{
    public class EqualizerController : IEqualizerController
    {
        public void ResetEqualizerControls(Grid channelsGrid)
        {
            ResetEqualizerControlsRecursive(channelsGrid);
        }

        private void ResetEqualizerControlsRecursive(DependencyObject parent)
        {
            if (parent == null) return;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Slider slider && slider.ToolTip is string tooltip)
                {
                    if (tooltip == "Bass" || tooltip == "Mid" || tooltip == "Treble")
                    {
                        slider.Value = 0; 
                    }
                }
                else
                {
                    ResetEqualizerControlsRecursive(child);
                }
            }
        }

        public void InitializeEqualizerControls(Grid channelsGrid, MainWindowController controller)
        {
            for (int i = 0; i < channelsGrid.Children.Count; i++)
            {
                var bands = controller.GetEqualizerBands(i);
                foreach (var band in bands)
                {
                    var slider = FindEqualizerSliderForChannel(channelsGrid, i, band.Frequency);
                    if (slider != null)
                    {
                        slider.Value = band.Gain; 
                    }
                }
            }
        }

        public void HandleEqualizerChanged(Slider slider)
        {
            if (slider.Tag is string tag && int.TryParse(tag, out int channelIndex))
            {
                float gain = (float)slider.Value;
                string frequencyName = slider.ToolTip?.ToString();

                // Map frequency names to actual frequencies
                float frequency = frequencyName switch
                {
                    "Bass" => 100f,
                    "Mid" => 1000f,
                    "Treble" => 5000f,
                    _ => throw new InvalidOperationException("Unknown frequency")
                };

                AppManager.Instance.GetChannelManager().UpdateEqualizerBand(channelIndex, frequency, gain);
            }
        }

        private Slider? FindEqualizerSliderForChannel(Grid channelsGrid, int channelIndex, float frequency)
        {
            foreach (var child in channelsGrid.Children)
            {
                if (child is Grid channelGrid)
                {
                    foreach (var gridChild in channelGrid.Children)
                    {
                        if (gridChild is Slider slider && slider.Tag?.ToString() == channelIndex.ToString())
                        {
                            string tooltip = slider.ToolTip?.ToString();
                            if (tooltip == "Bass" && frequency == 100f ||
                                tooltip == "Mid" && frequency == 1000f ||
                                tooltip == "Treble" && frequency == 5000f)
                            {
                                return slider;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
