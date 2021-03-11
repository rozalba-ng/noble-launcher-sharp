using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Controllers;
using System;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp {
    public partial class MainWindow : Window
    {
        private readonly SliderBlockController SliderController;
        private readonly SettingsBlockController SettingsController;
        private readonly ElementSearcherController ElementSearcher;
        public MainWindow()
        {
            InitializeComponent();
            ElementSearcher = ElementSearcherController.Init(this);
            SliderController = SliderBlockController.Init();
            SettingsController = SettingsBlockController.Init();
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
            SettingsController.ToggleVisibility();
        }

        private void OnCustomPatchSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());

            var patch = Globals.CustomPatches.GetPatchByID(id);
            var patchController = new NoblePatchController(patch);
            patchController.ToggleSelection();

            var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
            var settingsScrollerView = (ScrollViewer)ElementSearcher.FindName("SettingsScrollerView");
            var offset = settingsScrollerView.VerticalOffset;
            customPatchesView.Items.Refresh();
            settingsScrollerView.ScrollToVerticalOffset(offset);
        }
    }
}
