using NobleLauncher.Models;
using NobleLauncher.Structures;
using System.Windows.Controls;
using System.Windows.Input;

namespace NobleLauncher.Components
{
    /// <summary>
    /// Логика взаимодействия для UpdateButton.xaml
    /// </summary>
    public partial class UpdateButton : UserControl
    {
        public UpdateButton() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, UpdateCompleted);
        }

        public void StartUpdate(object sender, MouseButtonEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.StartUpdate);
            UpdateButtonTextView.Opacity = 0;
            UpdateButtonPreloaderView.Opacity = 0.5;
            UpdateButtonContainerView.IsHitTestVisible = false;
        }

        public void UpdateCompleted() {
            UpdateButtonTextView.Opacity = 1;
            UpdateButtonPreloaderView.Opacity = 0;
            UpdateButtonContainerView.IsHitTestVisible = true;
        }
    }
}
