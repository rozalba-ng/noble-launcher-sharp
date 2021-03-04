using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Controllers
{
    public class SliderBlockController
    {
        private static SliderBlockController instance;

        public bool IsOnSlide = false;
        private int currentImageIndex = -1;
        private Image CurrentImage;
        private Image MovingImage;
        private TextBlock CurrentSliderName;
        private readonly MainWindow Window;
        private SliderBlockController(MainWindow Window) {
            this.Window = Window;
            GetImageControllers();

            CurrentImage.Source = Globals.SliderElements[0].Image;
            CurrentImage.Tag = Globals.SliderElements[0].Link;
            CurrentSliderName.Text = Globals.SliderElements[0].Name;
            currentImageIndex = 0;
        }


        public static SliderBlockController Init(MainWindow Window) {
            if (instance == null) {
                instance = new SliderBlockController(Window);
            }

            return instance;
        }

        public void SlideToPrevious() {
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

        public void SlideToNext() {
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
        public void OnSlideCompleted() {
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
                for (int i = 0; i < Globals.SliderElements.Count; i++) {
                    if (CurrentActiveImage.ToString() == Globals.SliderElements[i].Image.ToString()) {
                        currentImageIndex = i;
                        break;
                    }
                }
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
            CurrentImage = (Image)Window.FindName("SquareCurrent");
            MovingImage = (Image)Window.FindName("SquareMoving");
            CurrentSliderName = (TextBlock)Window.FindName("SquareName");
        }

        private void PlayRightToLeftTransition() {
            IsOnSlide = true;
            Storyboard fadeOutAnim = (Storyboard)Window.FindResource("RightToLeft");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }

        private void PlayLeftToRightTransition() {
            IsOnSlide = true;
            Storyboard fadeOutAnim = (Storyboard)Window.FindResource("LeftToRight");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
