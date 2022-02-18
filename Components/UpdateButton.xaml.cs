using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для UpdateButton.xaml
    /// </summary>
    public partial class UpdateButton : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        private readonly TextBlock UpdateBtnText;
        private readonly Image Preloader;
        private readonly Grid Container;
        public UpdateButton() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            UpdateBtnText = (TextBlock)ElementSearcher.FindName("UpdateButtonText");
            Preloader = (Image)ElementSearcher.FindName("UpdateButtonPreloader");
            Container = (Grid)ElementSearcher.FindName("UpdateButtonContainer");

            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, UpdateCompleted);
        }

        public void StartUpdate(object sender, MouseButtonEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.StartUpdate);
            UpdateBtnText.Opacity = 0;
            Preloader.Opacity = 0.5;
            Container.IsHitTestVisible = false;
        }

        public void UpdateCompleted() {
            UpdateBtnText.Opacity = 1;
            Preloader.Opacity = 0;
            Container.IsHitTestVisible = true;
        }
    }
}
