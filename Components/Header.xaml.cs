using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    public partial class Header : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        public Header() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, () => SetSettingsClickable(false));
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, () => SetSettingsClickable(true));
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }

        private void SetSettingsClickable(bool clickable) {
            var icon = (Image)ElementSearcher.FindName("SettingsIcon");
            icon.IsHitTestVisible = clickable;
            icon.Opacity = clickable ? 1 : 0.33;
        }

        private void CloseClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.CurrentUIOperation.Abort();
            FileDownloader.AbortAnyLoad();

            Static.Patches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));
            Static.CustomPatches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));

            Application.Current.Shutdown();
        }

        private void HandleDrag(object sender, System.Windows.Input.MouseEventArgs e) {
            Application.Current.MainWindow.DragMove();
        }
    }
}
