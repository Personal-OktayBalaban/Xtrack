using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Xtrack.MainWindowControls.VolumeControllers
{
    internal class VolumeAndMuteResetter
    {
        public void Reset(Grid channelsGrid)
        {
            foreach (var child in GetAllChildren(channelsGrid))
            {
                ResetControl(child);
            }
        }

        private void ResetControl(DependencyObject child)
        {
            switch (child)
            {
                case Slider slider:
                    ResetSlider(slider);
                    break;
                
                case CheckBox checkBox:
                    ResetCheckBox(checkBox);
                    break;
                
                default:
                    break;
            }
        }

        private void ResetSlider(Slider slider)
        {
            slider.Value = 0.5;
        }

        private void ResetCheckBox(CheckBox checkBox)
        {
            checkBox.IsChecked = true;
        }

        private IEnumerable<DependencyObject> GetAllChildren(DependencyObject parent)
        {
            int count = VisualTreeHelper.GetChildrenCount(parent);
            
            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                yield return child;

                foreach (var grandChild in GetAllChildren(child))
                {
                    yield return grandChild;
                }
            }
        }
    }
}
