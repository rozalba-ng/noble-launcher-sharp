using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Controllers;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        private ServerModel UpdateServer;
        private UpdateServerRequestController UpdateServerRequest;
        private static readonly ServerModel NobleServer = new ServerModel("https://noblegarden.net");
        private static readonly NobleRequestController NobleRequest = new NobleRequestController(NobleServer);
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            PreloaderTaskController PreloaderController = new PreloaderTaskController(this, NobleRequest);
            string updateServerIP = await PreloaderController.GetUpdateServerAddress();
            UpdateServer = new ServerModel($"http://{updateServerIP}");
            UpdateServerRequest = new UpdateServerRequestController(UpdateServer);
            PreloaderController.SetUpdateRequestController(UpdateServerRequest);
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
