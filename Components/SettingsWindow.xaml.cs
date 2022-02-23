using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Components
{
    public partial class SettingsWindow : UserControl
    {
        private bool IsOnScreen = false;
        public SettingsWindow() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsButtonClick, ToggleOnScreen);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsRefresh, Refresh);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, PlayHideAnimation);
        }

        public void ToggleOnScreen() {
            if (IsOnScreen) PlayHideAnimation();
            else PlayShowAnimation();
        }

        public void Refresh() {
            var offset = SettingsScrollerView.VerticalOffset;
            SettingsScrollerView.ScrollToVerticalOffset(offset);
        }

        private void PlayShowAnimation() {
            if (IsOnScreen)
                return;
            Storyboard showAnim = (Storyboard)FindResource("ShowSettings");
            if (showAnim != null) {
                showAnim.Begin();
                IsOnScreen = true;
            }
        }

        private void PlayHideAnimation() {
            if (!IsOnScreen)
                return;
            Storyboard hideAnim = (Storyboard)FindResource("HideSettings");
            if (hideAnim != null) {
                hideAnim.Begin();
                IsOnScreen = false;
            }
        }
    }
}
