using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public NoblegardenRequestController requestController = new NoblegardenRequestController();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            NoblegardenRequestResponse response = await requestController.GetUpdateServerAddress();
            if (response.isOK) {
                Storyboard fadeOutAnim = (Storyboard)FindResource("FadeOutModalBG");
                if (fadeOutAnim != null) {
                    fadeOutAnim.Begin();
                }
            }
        }
    }
}
