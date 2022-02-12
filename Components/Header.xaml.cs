using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Windows;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        public Header() {
            InitializeComponent();
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            //SettingsBlockController.ToggleVisibility();
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }
    }
}
