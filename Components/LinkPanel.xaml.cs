using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Controllers;

namespace NoblegardenLauncherSharp.Components
{
    public partial class LinkPanel : UserControl
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(LinkPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(LinkPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(LinkPanel), new PropertyMetadata(default(string)));
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(string), typeof(LinkPanel), new PropertyMetadata(new PropertyChangedCallback(OnTypePropertyChanged)));
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        private readonly ElementSearcherController ElementSearcher;
        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Type {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
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
            ElementSearcher = new ElementSearcherController(this);

            Task.Run(() => SetDiscordLink());
        }

        private void OpenLink(object sender, MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }

        static private void OnTypePropertyChanged(DependencyObject targetObject, DependencyPropertyChangedEventArgs e) {
            string newType = (string)e.NewValue;
            Task.Run(async () => {
                LinkPanel panel = targetObject as LinkPanel;
                switch (newType) {
                    case "Discord":
                    await panel.SetDiscordLink();
                    return;
                    case "VK":
                    await panel.SetVKLink();
                    return;
                    default:
                    throw new Exception("Not found panel with type " + newType);
                }
            });
        }

        private async Task SetDiscordLink() {
            var discordLinkResponse = await SiteAPI.GetActualDiscordLink();
            var link = discordLinkResponse.GetFormattedData();
            await ApplyTag(link);
        }
        private async Task SetVKLink() {
            var discordLinkResponse = await SiteAPI.GetActualVKLink();
            var link = discordLinkResponse.GetFormattedData();
            await ApplyTag(link);
        }

        private async Task ApplyTag(string tag) {
            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var view = (Rectangle)ElementSearcher.FindName("LinkPanelView");
                    view.Tag = tag;
                })
            );
        }
    }
}
