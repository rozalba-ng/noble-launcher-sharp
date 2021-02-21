using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NoblegardenLauncherSharp
{
    public class PreloaderController
    {
        private readonly MainWindow Window;
        private readonly TextBlock LoadingStepView;
        private readonly NoblegardenRequestController requestController = new NoblegardenRequestController();
        public PreloaderController(MainWindow Window, TextBlock LoadingStepView) {
            this.Window = Window;
            this.LoadingStepView = LoadingStepView;
        }

        public async Task LoadUpdateServerIPAddress() {
            LoadingStepView.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_SERVER_ADDRESS];
            var response = await requestController.GetUpdateServerAddress();
            if (!response.isOK)
                throw new Exception(response.err);
        }

        public async Task CheckLauncherVersion() {
            LoadingStepView.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.CHECK_LAUNCHER_VERSION];
            var response = await requestController.GetActualLauncherVersion();
            if (!response.isOK)
                throw new Exception(response.err);

            string version = response.GetDataAsJSON().version;
            if (version == "")
                throw new Exception("Сервер не вернул актуальную версию лаунчера");

            if (version != Globals.LAUNCHER_VERSION)
                throw new Exception("Используется неактуальная версия лаунчера");
        }
        public async Task DrawVisualInformation() {
            LoadingStepView.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_LAST_UPDATES];
            await Task.WhenAll(
                Task.Run(() => DrawCurrentOnline()),
                Task.Run(() => DrawLastNews())
            );
        }

        private async Task DrawCurrentOnline() {
            var onlineResponse = await requestController.GetOnlineCount();
            if (onlineResponse == null)
                return;

            var wordForms = new string[] { "игрок", "игрока", "игроков" };
            string text = "Не найдено";
            if (onlineResponse.isOK) {
                try {
                    int playersCount = Int32.Parse(onlineResponse.data);
                    string wordForm = Globals.GetRightWordForm(playersCount, wordForms);
                    text = $"{playersCount} {wordForm}";
                }
                catch {
                    text = onlineResponse.data;
                }
            }

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var onlineCountBlock = (TextBlock)Window.FindName("CurrentOnline");
                    onlineCountBlock.Text = text;
                })
            );
        }
        private async Task DrawLastNews() {
            var newsResponse = await requestController.GetLastNews();
            Debug.WriteLine((string)newsResponse.GetDataAsJSON()[0].title);
        }
    }
}
