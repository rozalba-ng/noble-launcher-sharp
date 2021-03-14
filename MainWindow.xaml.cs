using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Controllers;
using System;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        private readonly SliderBlockController SliderController;
        private readonly SettingsBlockController SettingsBlockController;
        public MainWindow()
        {
            InitializeComponent();
            ElementSearcherController.Init(this);
            SettingsModel.Init();
            SiteAPIModel.Init("https://noblegarden.net");
            SliderController = SliderBlockController.Init();
            SettingsBlockController = SettingsBlockController.Init();
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            PreloaderTaskController PreloaderController = new PreloaderTaskController();
            await PreloaderController.GetUpdateServerAddress();
            await PreloaderController.CheckLauncherVersion();
            await PreloaderController.DrawVisualInformation();
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
            SettingsBlockController.ToggleVisibility();
        }

        private void OnCustomPatchSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());
            SettingsBlockController.ToggleCustomPatchSelection(id);
        }
    }
}
