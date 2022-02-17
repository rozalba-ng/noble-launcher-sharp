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
        private ElementSearcher ElementSearcher;
        public Header() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, MakeUnclickable);
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }

        private void MakeUnclickable() {
            var icon = (Image)ElementSearcher.FindName("SettingsIcon");
            icon.IsHitTestVisible = false;
        }

        private void CloseClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.CurrentUIOperation.Abort();
            FileDownloader.AbortAnyLoad();

            Static.Patches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));
            Static.CustomPatches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));

            Application.Current.Shutdown();
        }
    }
}
