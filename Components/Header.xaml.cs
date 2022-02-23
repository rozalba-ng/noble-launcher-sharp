using NobleLauncher.Globals;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace NobleLauncher.Components
{
    public partial class Header : UserControl
    {
        public Header() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, () => SetSettingsClickable(false));
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, () => SetSettingsClickable(true));
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }

        private void SetSettingsClickable(bool clickable) {
            SettingsIcon.IsHitTestVisible = clickable;
            SettingsIcon.Opacity = clickable ? 1 : 0.33;
        }

        private void CloseClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.Shutdown();
        }

        private void HandleDrag(object sender, System.Windows.Input.MouseEventArgs e) {
            Application.Current.MainWindow.DragMove();
        }
    }
}
