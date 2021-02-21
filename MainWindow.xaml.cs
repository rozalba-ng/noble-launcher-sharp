using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Models;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        //private TextBlock currentLoadingStepText;
        private ServerModel UpdateServer;
        private UpdateServerRequestController UpdateServerRequest;
        private static readonly ServerModel NobleServer = new ServerModel("https://noblegarden.net");
        private static readonly NobleRequestController NobleRequest = new NobleRequestController(NobleServer);
        public MainWindow()
        {
            InitializeComponent();
        }

        /*
        private TextBlock GetCurrentLoadingStepView() {
            if (currentLoadingStepText != null) return currentLoadingStepText;

            currentLoadingStepText = (TextBlock)FindName("CurrentLoadingStep");
            return currentLoadingStepText;
        }
        */

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            /*
            var PreloaderController = new PreloaderController(this, GetCurrentLoadingStepView());
            await PreloaderController.LoadUpdateServerIPAddress();
            await PreloaderController.CheckLauncherVersion();
            await PreloaderController.DrawVisualInformation();
            PlaySuccessLoadAnimation();
            */
            var updateServerAdressResponse = await NobleRequest.GetUpdateServerAddress();
            string updateServerIP = (string)updateServerAdressResponse.GetFormattedData();
            UpdateServer = new ServerModel($"http://{updateServerIP}");
            UpdateServerRequest = new UpdateServerRequestController(UpdateServer);

            var launcherVersionResponse = await UpdateServerRequest.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.GetFormattedData().version;
            if (actualLauncherVersion != Globals.LAUNCHER_VERSION) {
                throw new Exception("Используется неактуальная версия лаунчера");
            }
        }

        private void PlaySuccessLoadAnimation() {
            /*
            GetCurrentLoadingStepView().Text = "";
            Storyboard fadeOutAnim = (Storyboard)FindResource("FadeOutModalBG");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
            */
        }
    }
}
