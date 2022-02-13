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

        public void Refresh() {
            var settingsScrollerView = (ScrollViewer)ElementSearcher.FindName("SettingsScrollerView");
            var offset = settingsScrollerView.VerticalOffset;
            settingsScrollerView.ScrollToVerticalOffset(offset);
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
