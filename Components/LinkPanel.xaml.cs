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
    /// Логика взаимодействия для LinkPanel.xaml
    /// </summary>
    public partial class LinkPanel : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LinkPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(LinkPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(LinkPanel), new PropertyMetadata(default(string)));

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public ImageSource Image {
            get { return (ImageSource)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }

        public Brush Fill {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public LinkPanel() {
            InitializeComponent();
        }

        private void OpenLink(object sender, MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }
    }
}
