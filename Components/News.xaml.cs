using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для News.xaml
    /// </summary>
    public partial class News : UserControl
    {
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        private readonly ElementSearcherController ElementSearcher;
        public News() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            Task.Run(() => DrawLastNews());
        }
        private async Task DrawLastNews() {
            var newsResponse = await SiteAPI.GetLastNews();
            var newsAsJSONArray = newsResponse.GetFormattedData();

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

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var newsListView = (ListView)ElementSearcher.FindName("LastNewsView");
                    newsListView.ItemsSource = newsList;
                })
            );
        }
    }
}
