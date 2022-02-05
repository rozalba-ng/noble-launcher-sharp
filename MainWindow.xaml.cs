using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Controllers;
using System;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        private readonly SettingsBlockController SettingsBlockController;
        public MainWindow()
        {
            InitializeComponent();
            SettingsModel.Init();
            ElementSearcherController.Init(this);
            SiteAPIModel.Init("https://noblegarden.net");
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

        private void OnCustomPatchSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());
            SettingsBlockController.ToggleCustomPatchSelection(id);
        }
    }
}
