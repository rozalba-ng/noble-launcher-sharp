using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Controllers;
using System;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        private ServerModel UpdateServer;
        private UpdateServerRequestController UpdateServerRequest;
        private readonly SliderBlockController SliderController;
        private readonly SettingsBlockController SettingsController;
        private static readonly ServerModel NobleServer = new ServerModel("https://noblegarden.net");
        private static readonly NobleRequestController NobleRequest = new NobleRequestController(NobleServer);
        public MainWindow()
        {
            InitializeComponent();
            SliderController = SliderBlockController.Init(this);
            SettingsController = SettingsBlockController.Init(this);
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            PreloaderTaskController PreloaderController = new PreloaderTaskController(this, NobleRequest);
            string updateServerIP = await PreloaderController.GetUpdateServerAddress();
            UpdateServer = new ServerModel($"http://{updateServerIP}");
            UpdateServerRequest = new UpdateServerRequestController(UpdateServer);
            PreloaderController.SetUpdateRequestController(UpdateServerRequest);
            await PreloaderController.CheckLauncherVersion();
            await PreloaderController.DrawVisualInformation();
            await PreloaderController.RequestPatches();
            PreloaderController.PlaySuccessLoadAnimation();
        }

        private void OpenLinkFromTag(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            string link = target.Tag.ToString();
            Process.Start(link);
        }

        private void OnSliderPreviousButtonClick(object sender, RoutedEventArgs e) {
            SliderController.SlideToPrevious();
        }

        private void OnSliderNextButtonClick(object sender, RoutedEventArgs e) {
            SliderController.SlideToNext();
        }
        private void OnSlideCompleted(object sender, EventArgs e) {
            SliderController.OnSlideCompleted();
        }

        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            SettingsController.ToggleVisibility();
        }
    }
}
