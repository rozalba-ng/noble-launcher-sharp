using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Controllers;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для CurrentOnline.xaml
    /// </summary>
    public partial class CurrentOnline : UserControl
    {
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        private readonly ElementSearcherController ElementSearcher;
        public CurrentOnline() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            Task.Run(() => DrawCurrentOnline());
        }
        private void OpenCurrentOnlineLink(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }

        private async Task DrawCurrentOnline() {
            var onlineResponse = await SiteAPI.GetOnlineCount();
            var wordForms = new string[] { "игрок", "игрока", "игроков" };
            string text = "Не найдено";
            if (onlineResponse.IsOK) {
                try {
                    int playersCount = Int32.Parse(onlineResponse.GetFormattedData());
                    string wordForm = Globals.GetRightWordForm(playersCount, wordForms);
                    text = $"{playersCount} {wordForm}";
                }
                catch {
                    text = onlineResponse.GetFormattedData();
                }
            }

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var onlineCountBlock = (TextBlock)ElementSearcher.FindName("CurrentOnlineText");
                    onlineCountBlock.Text = text;
                })
            );
        }
    }
}
