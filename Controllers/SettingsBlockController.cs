using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Controllers
{
    class SettingsBlockController
    {
        private static SettingsBlockController instance;

        private bool IsVisible = false;
        private Grid View;
        private MainWindow Window;

        private SettingsBlockController(MainWindow Window) {
            this.Window = Window;
            FindView();
        }

        public static SettingsBlockController Init(MainWindow Window) {
            if (instance == null) {
                instance = new SettingsBlockController(Window);
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

        private void FindView() {
            View = (Grid)Window.FindName("SettingsView");
        }
        private void PlayShowAnimation() {
            Storyboard showAnim = (Storyboard)Window.FindResource("ShowSettings");
            if (showAnim != null) {
                showAnim.Begin();
            }
        }

        private void PlayHideAnimation() {
            Storyboard hideAnim = (Storyboard)Window.FindResource("HideSettings");
            if (hideAnim != null) {
                hideAnim.Begin();
            }
        }
    }
}
