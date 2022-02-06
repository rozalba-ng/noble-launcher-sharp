using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Structures;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Components
{
    public partial class SettingsWindow : UserControl
    {
        private bool IsOnScreen = false;
        private readonly ElementSearcherController ElementSearcher;
        public SettingsWindow() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsButtonClick, ToggleOnScreen);
        }

        public void ToggleOnScreen() {
            if (IsOnScreen) {
                PlayHideAnimation();
            }
            else {
                PlayShowAnimation();
            }
            IsOnScreen = !IsOnScreen;
        }
        private void PlayShowAnimation() {
            Storyboard showAnim = ElementSearcher.FindStoryboard("ShowSettings");
            if (showAnim != null) {
                showAnim.Begin();
            }
        }

        private void PlayHideAnimation() {
            Storyboard hideAnim = ElementSearcher.FindStoryboard("HideSettings");
            if (hideAnim != null) {
                hideAnim.Begin();
            }
        }
    }
}
