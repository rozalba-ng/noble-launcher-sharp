using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для CurrentOnline.xaml
    /// </summary>
    public partial class CurrentOnline : UserControl
    {
        public CurrentOnline() {
            InitializeComponent();
        }
        private void OpenCurrentOnlineLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }
    }
}
