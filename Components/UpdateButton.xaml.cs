using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Interfaces;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для UpdateButton.xaml
    /// </summary>
    public partial class UpdateButton : UserControl
    {
        public UpdateButton() {
            InitializeComponent();
        }

        public void StartUpdate(object sender, MouseButtonEventArgs e) {
            EventDispatcher.Dispatch(EventDispatcherEvent.StartUpdate);
        }
    }
}
