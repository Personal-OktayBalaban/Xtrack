using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace Xtrack.MainWindowControls.EqualizerControllers
{
    internal class EqualizerResetter
    {
        public void Reset(Grid channelsGrid)
        {
            ResetRecursive(channelsGrid);
        }

        private void ResetRecursive(DependencyObject parent)
        {
            if (parent == null)
            {
                return;
            }

            ResetSliders(parent);
        }

        private void ResetSliders(DependencyObject parent)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);

            for (int i = 0; i < childCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);

                if (child is Slider slider && IsEqualizerControl(slider))
                {
                    slider.Value = 0;
                }
                else
                {
                    ResetSliders(child);
                }
            }
        }

        private bool IsEqualizerControl(Slider slider)
        {
            if (slider.ToolTip is not string tooltip)
            {
                return false;
            }

            return tooltip == "Bass" || tooltip == "Mid" || tooltip == "Treble";
        }
    }
}
