using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для StartButton.xaml
    /// </summary>
    public partial class StartButton : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        public StartButton() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, () => ToggleButtonState(false));
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompleteUpdate, () => ToggleButtonState(true));

            if (!IsEXEPresented()) {
                ToggleButtonState(false);
            }
        }

        private bool IsEXEPresented() {
            string pathToEXE = Settings.WORKING_DIR + "/Wow.exe";
            return File.Exists(pathToEXE);
        }

        private void ToggleButtonState(bool enabled) {
            var container = (Grid)ElementSearcher.FindName("StartButtonContainer");
            var bg = (Rectangle)ElementSearcher.FindName("StartButtonBg");
            var buttonBrush = new SolidColorBrush {
                Color = enabled ? Color.FromRgb(80, 190, 60) : Color.FromRgb(170, 170, 170)
            };
            container.IsHitTestVisible = enabled;
            bg.Fill = buttonBrush;
        }

        private void Start(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (!IsEXEPresented())
                return;

            if (Settings.WORKING_DIR == ".") {
                Process.Start("Wow.exe");
            } else {
                Process.Start(Settings.WORKING_DIR + "/Wow.exe");
            }
        }
    }
}
