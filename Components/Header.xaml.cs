using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Windows;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    public partial class Header : UserControl
    {
        public Header() {
            InitializeComponent();
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }

        private void CloseClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
