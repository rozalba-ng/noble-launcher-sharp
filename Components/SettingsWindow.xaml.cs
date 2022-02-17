using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Components
{
    public partial class SettingsWindow : UserControl
    {
        private bool IsOnScreen = false;
        private readonly ElementSearcher ElementSearcher;
        public SettingsWindow() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsButtonClick, ToggleOnScreen);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.SettingsRefresh, Refresh);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, PlayHideAnimation);
        }

        public void ToggleOnScreen() {
            if (IsOnScreen) PlayHideAnimation();
            else PlayShowAnimation();
        }

        public void Refresh() {
            var settingsScrollerView = (ScrollViewer)ElementSearcher.FindName("SettingsScrollerView");
            var offset = settingsScrollerView.VerticalOffset;
            settingsScrollerView.ScrollToVerticalOffset(offset);
        }

        private void PlayShowAnimation() {
            if (IsOnScreen)
                return;
            Storyboard showAnim = ElementSearcher.FindStoryboard("ShowSettings");
            if (showAnim != null) {
                showAnim.Begin();
                IsOnScreen = true;
            }
        }

        private void PlayHideAnimation() {
            if (!IsOnScreen)
                return;
            Storyboard hideAnim = ElementSearcher.FindStoryboard("HideSettings");
            if (hideAnim != null) {
                hideAnim.Begin();
                IsOnScreen = false;
            }
        }
    }
}
