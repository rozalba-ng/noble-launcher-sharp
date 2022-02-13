using System.Windows;
using System.Diagnostics;
using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;

namespace NoblegardenLauncherSharp {
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
