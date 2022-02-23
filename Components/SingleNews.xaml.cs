using NobleLauncher.Globals;
using System.Windows.Controls;

namespace NobleLauncher.Components
{
    public partial class SingleNews : UserControl
    {
        public SingleNews() {
            InitializeComponent();
        }

        private void OpenNewsLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Static.OpenLinkFromTag(sender, e);
        }
    }
}
