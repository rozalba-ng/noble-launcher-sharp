using NoblegardenLauncherSharp.Controllers;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Slider.xaml
    /// </summary>
    public partial class Slider : UserControl
    {
        private readonly SliderBlockController SliderController;
        public Slider() {
            InitializeComponent();
            SliderController = SliderBlockController.Init();
        }
        private void OpenSliderLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }

        private void OnSliderPreviousButtonClick(object sender, RoutedEventArgs e) {
            //SliderController.SlideToPrevious();
        }

        private void OnSliderNextButtonClick(object sender, RoutedEventArgs e) {
            //SliderController.SlideToNext();
        }

        private void OnSlideCompleted(object sender, EventArgs e) {
            //SliderController.OnSlideCompleted();
        }
    }
}
