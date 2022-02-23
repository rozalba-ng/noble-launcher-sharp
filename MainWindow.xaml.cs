using System.Windows;
using System.Diagnostics;
using NobleLauncher.Globals;
using NobleLauncher.Models;
using NobleLauncher.Structures;

namespace NobleLauncher {
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Settings.Parse();
            InitializeComponent();
        }

        private void OnWindowLoad(object sender, RoutedEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.StartPreload);
        }
    }
}
