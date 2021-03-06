using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class PreloaderTaskController
    {
        private readonly MainWindow LauncherWindow;
        private readonly NobleRequestController NobleRequest;
        private UpdateServerRequestController UpdateRequest;
        private TextBlock CurrentLoadingStepText;
        public PreloaderTaskController (MainWindow LauncherWindow, NobleRequestController NobleRequest) {
            this.NobleRequest = NobleRequest;
            this.LauncherWindow = LauncherWindow;
            GetCurrentLoadingStepView();
        }

        public void SetUpdateRequestController(UpdateServerRequestController UpdateRequest) {
            this.UpdateRequest = UpdateRequest;
        }
        public async Task<string> GetUpdateServerAddress() {
            CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_SERVER_ADDRESS];
            var updateServerAdressResponse = await NobleRequest.GetUpdateServerAddress();
            string updateServerIP = (string)updateServerAdressResponse.GetFormattedData();
            return updateServerIP;
        }

        public async Task CheckLauncherVersion() {
            CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.CHECK_LAUNCHER_VERSION];
            var launcherVersionResponse = await UpdateRequest.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.GetFormattedData().version;
            if (actualLauncherVersion == "") {
                throw new Exception("Сервер не вернул актуальной версии лаунчера");
            }
            if (actualLauncherVersion != Globals.LAUNCHER_VERSION) {
                throw new Exception("Используется неактуальная версия лаунчера");
            }
        }

        public async Task DrawVisualInformation() {
            CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_LAST_UPDATES];
            await Task.WhenAll(
                Task.Run(() => DrawCurrentOnline()),
                Task.Run(() => DrawLastNews()),
                Task.Run(() => SetDiscordLink()),
                Task.Run(() => SetVKLink())
            );
        }

        public async Task RequestPatches() {
            CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_PATCHES_INFO];
            await GetAndDrawCustomPatches();
        }

        public void PlaySuccessLoadAnimation() {
            CurrentLoadingStepText.Text = "";
            Storyboard fadeOutAnim = (Storyboard)LauncherWindow.FindResource("FadeOutModalBG");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }

        private async Task DrawCurrentOnline() {
            var onlineResponse = await NobleRequest.GetOnlineCount();
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
                    var onlineCountBlock = (TextBlock)LauncherWindow.FindName("CurrentOnline");
                    onlineCountBlock.Text = text;
                })
            );
        }
        private async Task DrawLastNews() {
            var newsResponse = await NobleRequest.GetLastNews();
            var newsAsJSONArray = newsResponse.GetFormattedData();

            var newsList = new List<SingleNewsModel>();
            for (int i = 0; i < newsAsJSONArray.Count; i++) {
                var current = newsAsJSONArray[i];
                newsList.Add(
                    new SingleNewsModel(
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
                    var newsListView = (ListView)LauncherWindow.FindName("LastNewsView");
                    newsListView.ItemsSource = newsList;
                })
            );
        }

        private async Task SetDiscordLink() {
            var discordLinkResponse = await NobleRequest.GetActualDiscordLink();
            var link = discordLinkResponse.GetFormattedData();

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var discordView = (Rectangle)LauncherWindow.FindName("DiscordView");
                    discordView.Tag = link;
                })
            );
        }
        private async Task SetVKLink() {
            var discordLinkResponse = await NobleRequest.GetActualVKLink();
            var link = discordLinkResponse.GetFormattedData();

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var vkView = (Rectangle)LauncherWindow.FindName("VKView");
                    vkView.Tag = link;
                })
            );
        }

        private async Task GetAndDrawCustomPatches() {
            var customPatchesResponse = await UpdateRequest.GetCustomPatches();
            var customPatches = customPatchesResponse.GetFormattedData();
            var tokens = JObjectConverter.ConvertToPatch(customPatches);
            Globals.Patches.AddRange(tokens);
        } 
        private TextBlock GetCurrentLoadingStepView() {
            if (CurrentLoadingStepText != null)
                return CurrentLoadingStepText;

            CurrentLoadingStepText = (TextBlock)LauncherWindow.FindName("CurrentLoadingStep");
            return CurrentLoadingStepText;
        }
    }
}
