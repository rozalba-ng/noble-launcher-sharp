using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using NobleLauncher.Globals;
using System.Windows.Media.Animation;

namespace NobleLauncher.Components
{
    public partial class Slider : UserControl
    {
        public bool IsOnSlide = false;
        private int currentImageIndex = 0;

        public Slider() {
            InitializeComponent();
        }

        private void OpenSliderLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.OpenLinkFromTag(sender, e);
        }

        private void OnSliderPreviousButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int previousIndex = GetPreviousImageIndex();
            SquareMovingView.Source = Static.SliderElements[previousIndex].Image;
            SquareMovingView.Tag = Static.SliderElements[previousIndex].Link;
            SquareMovingView.Visibility = Visibility.Visible;
            currentImageIndex = previousIndex;
            SquareNameView.Text = Static.SliderElements[previousIndex].Name;
            PlayRightToLeftTransition();
        }

        private void OnSliderNextButtonClick(object sender, RoutedEventArgs e) {
            if (IsOnSlide)
                return;
            int nextIndex = GetNextImageIndex();
            SquareMovingView.Source = Static.SliderElements[nextIndex].Image;
            SquareMovingView.Tag = Static.SliderElements[nextIndex].Link;
            SquareMovingView.Visibility = Visibility.Visible;
            currentImageIndex = nextIndex;
            SquareNameView.Text = Static.SliderElements[nextIndex].Name;
            PlayLeftToRightTransition();
        }

        private void OnSlideCompleted(object sender, EventArgs e) {
            if (!IsOnSlide)
                return;
            SquareCurrentView.Source = SquareMovingView.Source;
            SquareCurrentView.Tag = SquareMovingView.Tag;
            SquareMovingView.Visibility = Visibility.Hidden;
            IsOnSlide = false;
        }

        private int GetCurrentImageIndex() {
            var CurrentActiveImage = SquareCurrentView.Source;

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
            var fadeOutAnim = (Storyboard)FindResource("RightToLeft");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }

        private void PlayLeftToRightTransition() {
            IsOnSlide = true;
            var fadeOutAnim = (Storyboard)FindResource("LeftToRight");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
