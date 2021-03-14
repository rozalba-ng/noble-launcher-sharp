using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Controllers
{
    class SettingsBlockController
    {
        private static SettingsBlockController instance;

        private bool IsVisible = false;
        private readonly ElementSearcherController ElementSearcher;

        private SettingsBlockController() {
            ElementSearcher = ElementSearcherController.GetInstance();
        }

        public static SettingsBlockController Init() {
            if (instance == null) {
                instance = new SettingsBlockController();
            }

            return instance;
        }

        public void ToggleVisibility() {
            if (IsVisible) {
                PlayHideAnimation();
            } else {
                PlayShowAnimation();
            }
            IsVisible = !IsVisible;
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

        public void ToggleCustomPatchSelection(int customPatchID) {
            var patch = Globals.CustomPatches.GetPatchByID(customPatchID);
            patch.ChangeSelectionTo(!patch.Selected);
            var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
            var settingsScrollerView = (ScrollViewer)ElementSearcher.FindName("SettingsScrollerView");
            var offset = settingsScrollerView.VerticalOffset;
            customPatchesView.Items.Refresh();
            settingsScrollerView.ScrollToVerticalOffset(offset);
        }
    }
}
