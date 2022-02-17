using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
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
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, DisableButton);

            if (!IsEXEPresented()) {
                DisableButton();
            }
        }

        private bool IsEXEPresented() {
            string pathToEXE = Settings.WORKING_DIR + "/Wow.exe";
            return File.Exists(pathToEXE);
        }

        private void DisableButton() {
            var container = (Grid)ElementSearcher.FindName("StartButtonContainer");
            var bg = (Rectangle)ElementSearcher.FindName("StartButtonBg");
            var buttonBrush = new SolidColorBrush {
                Color = Color.FromRgb(170, 170, 170)
            };
            container.IsHitTestVisible = false;
            bg.Fill = buttonBrush;
        }
    }
}
