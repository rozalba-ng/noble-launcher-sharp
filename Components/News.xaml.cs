using NobleLauncher.Globals;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NobleLauncher.Components
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News : UserControl
    {
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        public News() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompletePreload, () => Task.Run(() => DrawLastNews()));
        }
        private async Task DrawLastNews() {
            var newsResponse = await SiteAPI.GetLastNews();
            var newsAsJSONArray = newsResponse.FormattedData;

            var newsList = new List<SitePost>();
            for (int i = 0; i < newsAsJSONArray.Count; i++) {
                var current = newsAsJSONArray[i];
                newsList.Add(
                    new SitePost(
                        (string)current.title,
                        (string)current.preview,
                        (string)current.link,
                        (string)current.author,
                        (string)current.authorLink
                   )
                );
            }

            Static.InUIThread(() => {
                LastNewsView.ItemsSource = newsList;
            });
        }
    }
}
