using NoblegardenLauncherSharp.Controllers;
using System.Windows;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Header.xaml
    /// </summary>
    public partial class Header : UserControl
    {
        private readonly SettingsBlockController SettingsBlockController;
        public Header() {
            InitializeComponent();
            SettingsBlockController = SettingsBlockController.Init();
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            SettingsBlockController.ToggleVisibility();
        }
    }
}
