using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ReadingBar
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void BarOpacitySlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BarOpacityLabel.Content = $"Bar Opacity: {e.NewValue}%";
        }

        private void BarSizeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BarSizeLabel.Content = $"Bar Size: {e.NewValue}";
        }

        private void BGShadingSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BGShadingLabel.Content = $"Background Shading Opacity: {e.NewValue}%";
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
