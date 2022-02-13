using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Components
{
    public partial class Slider : UserControl
    {
        public bool IsOnSlide = false;
        private int currentImageIndex = 0;
        private readonly Image CurrentImage;
        private readonly Image MovingImage;
        private readonly TextBlock CurrentSliderName;
        private readonly ElementSearcher ElementSearcher;

        public Slider() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            CurrentImage = (Image)ElementSearcher.FindName("SquareCurrent");
            MovingImage = (Image)ElementSearcher.FindName("SquareMoving");
            CurrentSliderName = (TextBlock)ElementSearcher.FindName("SquareName");
        }

        private void OpenSliderLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.OpenLinkFromTag(sender, e);
        }

        private void OnSliderPreviousButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int previousIndex = GetPreviousImageIndex();
            MovingImage.Source = Static.SliderElements[previousIndex].Image;
            MovingImage.Tag = Static.SliderElements[previousIndex].Link;
            MovingImage.Visibility = Visibility.Visible;
            currentImageIndex = previousIndex;
            CurrentSliderName.Text = Static.SliderElements[previousIndex].Name;
            PlayRightToLeftTransition();
        }

        private void OnSliderNextButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int nextIndex = GetNextImageIndex();
            MovingImage.Source = Static.SliderElements[nextIndex].Image;
            MovingImage.Tag = Static.SliderElements[nextIndex].Link;
            MovingImage.Visibility = Visibility.Visible;
            currentImageIndex = nextIndex;
            CurrentSliderName.Text = Static.SliderElements[nextIndex].Name;
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
                Parallel.For(0, Static.SliderElements.Count, (i) => {
                    if (CurrentActiveImage.ToString() == Static.SliderElements[i].Image.ToString()) {
                        currentImageIndex = i;
                    }
                });
            }

            return currentImageIndex;
        }

        private int GetNextImageIndex() {
            int currentIndex = GetCurrentImageIndex();
            if (currentIndex == Static.SliderElements.Count - 1) {
                return 0;
            }

            return currentIndex + 1;
        }

        private int GetPreviousImageIndex() {
            int currentIndex = GetCurrentImageIndex();
            if (currentIndex == 0) {
                return Static.SliderElements.Count - 1;
            }

            return currentIndex - 1;
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
