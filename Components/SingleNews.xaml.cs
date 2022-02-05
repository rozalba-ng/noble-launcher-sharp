using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для SingleNews.xaml
    /// </summary>
    public partial class SingleNews : UserControl
    {
        public SingleNews() {
            InitializeComponent();
        }

        private void OpenNewsLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }
    }
}
