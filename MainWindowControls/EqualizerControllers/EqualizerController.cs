using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;
using System.Windows;
using Xtrack.Channel;
using Xtrack.MainWindowControls.EqualizerControllers;

namespace Xtrack.MainWindowControls
{
    public class EqualizerController : IEqualizerController
    {
        private readonly EqualizerResetter _resetter;
        private readonly EqualizerInitializer _initializer;
        private readonly EqualizerChangeHandler _changeHandler;

        public EqualizerController() 
        {
            _resetter = new EqualizerResetter();
            _initializer = new EqualizerInitializer();
            _changeHandler = new EqualizerChangeHandler();
        }

        public void ResetEqualizerControls(Grid channelsGrid)
        {
            _resetter.Reset(channelsGrid);
        }

        public void InitializeEqualizerControls(Grid channelsGrid, MainWindowController controller)
        {
            _initializer.Initialize(channelsGrid, controller);
        }

        public void HandleEqualizerChanged(Slider slider)
        {
            _changeHandler.Handle(slider);
        }
    }
}
