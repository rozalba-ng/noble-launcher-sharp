using NobleLauncher.Models;
using NobleLauncher.Structures;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NobleLauncher.Components
{
    public partial class AddonsWindow : UserControl
    {
        private bool IsOnScreen = false;
        public AddonsWindow() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsButtonClick, ToggleOff);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.AddonsButtonClick, ToggleOnScreen);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.AddonsRefresh, Refresh);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, PlayHideAnimation);
        }

        public void ToggleOnScreen() {
            if (IsOnScreen) PlayHideAnimation();
            else PlayShowAnimation();
        }
        public void ToggleOff()
        {
            if (IsOnScreen) PlayHideAnimation();
        }

        public void Refresh() {
            var offset = AddonsScrollerView.VerticalOffset;
            AddonsScrollerView.ScrollToVerticalOffset(offset);
        }

        private void PlayShowAnimation() {
            if (IsOnScreen)
                return;
            Storyboard showAnim = (Storyboard)FindResource("ShowAddons");
            if (showAnim != null) {
                showAnim.Begin();
                IsOnScreen = true;
            }
        }

        private void PlayHideAnimation() {
            if (!IsOnScreen)
                return;
            Storyboard hideAnim = (Storyboard)FindResource("HideAddons");
            if (hideAnim != null) {
                hideAnim.Begin();
                IsOnScreen = false;
            }
        }
    }
}
