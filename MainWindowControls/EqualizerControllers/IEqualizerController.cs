using System.Windows.Controls;

namespace Xtrack.MainWindowControls.EqualizerControllers
{
    public interface IEqualizerController
    {
        void ResetEqualizerControls(Grid channelsGrid);
        void InitializeEqualizerControls(Grid channelsGrid, MainWindowController controller);
        void HandleEqualizerChanged(Slider slider);
    }
}
