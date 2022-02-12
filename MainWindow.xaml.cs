using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Globals;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Settings.Parse();
            InitializeComponent();
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            PreloaderTaskController PreloaderController = new PreloaderTaskController();
            await PreloaderController.CheckLauncherVersion();
            await PreloaderController.DrawVisualInformation();
            PreloaderController.PlaySuccessLoadAnimation();
        }

        private void OpenLinkFromTag(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            string link = target.Tag.ToString();
            Process.Start(link);
        }
    }
}
