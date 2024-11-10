using NobleLauncher.Globals;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NobleLauncher.Components
{
    public partial class Header : UserControl
    {
        public Header() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompletePreload, () =>
            {
                SetSettingsButtonVisible();
                SetAddonsButtonVisible();
            });
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, () =>
            {
                SetSettingsClickable(false);
                SetAddonsClickable(false);
            });
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, () =>
            {
                SetSettingsClickable(true);
                SetAddonsClickable(true);
            });
        }

        private void SetSettingsButtonVisible()
        {
            SettingsIcon.Visibility = Visibility.Visible;
            SetSettingsClickable(true);
        }
        private void SetAddonsButtonVisible()
        {
            AddonsIcon.Visibility = Visibility.Visible;
            SetAddonsClickable(true);
        }
        private void ToggleSettingsVisibility(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsButtonClick);
        }
        
        private void SetSettingsClickable(bool clickable) {
            SettingsIcon.IsHitTestVisible = clickable;
            if (clickable) SettingsIcon.ClearValue(OpacityProperty);
            else SettingsIcon.SetValue(OpacityProperty, 0.33);
        }
        private void SetAddonsClickable(bool clickable)
        {
            AddonsIcon.IsHitTestVisible = clickable;
            if (clickable) AddonsIcon.ClearValue(OpacityProperty);
            else AddonsIcon.SetValue(OpacityProperty, 0.33);
        }

        private void CloseClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.Shutdown();
        }

        private void HandleDrag(object sender, System.Windows.Input.MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Application.Current.MainWindow.DragMove();
            }
        }

        private void ToggleAddonsVisibility(object sender, MouseButtonEventArgs e)
        {
            EventDispatcher.Dispatch(EventDispatcherEvent.AddonsButtonClick);
        }
    }
}
