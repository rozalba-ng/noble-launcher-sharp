using NoblegardenLauncherSharp.Controllers;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    public partial class Slider : UserControl
    {
        public bool IsOnSlide = false;
        private int currentImageIndex = 0;
        private Image CurrentImage;
        private Image MovingImage;
        private TextBlock CurrentSliderName;
        private readonly ElementSearcherController ElementSearcher;

        public Slider() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            GetImageControllers();
            CurrentImage.Source = Globals.SliderElements[0].Image;
            CurrentImage.Tag = Globals.SliderElements[0].Link;
            CurrentSliderName.Text = Globals.SliderElements[0].Name;
        }
        private void OpenSliderLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }

        private void OnSliderPreviousButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int previousIndex = GetPreviousImageIndex();
            MovingImage.Source = Globals.SliderElements[previousIndex].Image;
            MovingImage.Tag = Globals.SliderElements[previousIndex].Link;
            MovingImage.Visibility = Visibility.Visible;
            currentImageIndex = previousIndex;
            CurrentSliderName.Text = Globals.SliderElements[previousIndex].Name;
            PlayRightToLeftTransition();
        }

        private void OnSliderNextButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int nextIndex = GetNextImageIndex();
            MovingImage.Source = Globals.SliderElements[nextIndex].Image;
            MovingImage.Tag = Globals.SliderElements[nextIndex].Link;
            MovingImage.Visibility = Visibility.Visible;
            currentImageIndex = nextIndex;
            CurrentSliderName.Text = Globals.SliderElements[nextIndex].Name;
            PlayLeftToRightTransition();
        }

        private void OnSlideCompleted(object sender, EventArgs e) {
            if (!IsOnSlide)
                return;
            CurrentImage.Source = MovingImage.Source;
            CurrentImage.Tag = MovingImage.Tag;
            MovingImage.Visibility = Visibility.Hidden;
            IsOnSlide = false;
        }

        private int GetCurrentImageIndex() {
            var CurrentActiveImage = CurrentImage.Source;

            if (currentImageIndex == -1) {
                Parallel.For(0, Globals.SliderElements.Count, (i) => {
                    if (CurrentActiveImage.ToString() == Globals.SliderElements[i].Image.ToString()) {
                        currentImageIndex = i;
                    }
                });
            }

            return currentImageIndex;
        }

        private int GetNextImageIndex() {
            int currentIndex = GetCurrentImageIndex();
            if (currentIndex == Globals.SliderElements.Count - 1) {
                return 0;
            }

            return currentIndex + 1;
        }

        private int GetPreviousImageIndex() {
            int currentIndex = GetCurrentImageIndex();
            if (currentIndex == 0) {
                return Globals.SliderElements.Count - 1;
            }

            return currentIndex - 1;
        }

        private void GetImageControllers() {
            CurrentImage = (Image)ElementSearcher.FindName("SquareCurrent");
            MovingImage = (Image)ElementSearcher.FindName("SquareMoving");
            CurrentSliderName = (TextBlock)ElementSearcher.FindName("SquareName");
        }

        private void PlayRightToLeftTransition() {
            IsOnSlide = true;
            var fadeOutAnim = ElementSearcher.FindStoryboard("RightToLeft");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }

        private void PlayLeftToRightTransition() {
            IsOnSlide = true;
            var fadeOutAnim = ElementSearcher.FindStoryboard("LeftToRight");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
