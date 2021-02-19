using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private NoblegardenRequestController requestController = new NoblegardenRequestController();
        private TextBlock currentLoadingStepText;
        public MainWindow()
        {
            InitializeComponent();
        }

        private TextBlock currentLoadingStep() {
            if (currentLoadingStepText != null) return currentLoadingStepText;

            currentLoadingStepText = (TextBlock)FindName("CurrentLoadingStep");
            return currentLoadingStepText;
        }

        private async void OnWindowLoad(object sender, RoutedEventArgs e) {
            currentLoadingStep().Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_SERVER_ADDRESS];
            var response = await requestController.GetUpdateServerAddress();
            if (!response.isOK)
                throw new Exception(response.err);

            OnServerAddressKnown();
        }

        private async void OnServerAddressKnown() {
            currentLoadingStep().Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.CHECK_LAUNCHER_VERSION];
            var response = await requestController.GetActualLauncherVersion();
            if (!response.isOK)
                throw new Exception(response.err);

            string version = response.getDataAsJSON().version;
            if (version == "")
                throw new Exception("Сервер не вернул актуальную версию лаунчера");

            if (version != Globals.LAUNCHER_VERSION)
                throw new Exception("Используется неактуальная версия лаунчера");

            RequestLauncherVisualInformation();
        }

        private async void RequestLauncherVisualInformation() {
            currentLoadingStep().Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_LAST_UPDATES];

            var onlineResponse = await requestController.GetOnlineCount();
            var newsResponse = await requestController.GetLastNews();

            Debug.WriteLine(onlineResponse.data);
            Debug.WriteLine((string)newsResponse.getDataAsJSON()[0].title);
            DrawCurrentOnline(onlineResponse);
            PlaySuccessLoadAnimation();
        }

        private void DrawCurrentOnline(NoblegardenRequestResponse onlineResponse) {
            if (onlineResponse == null)
                return;

            var onlineCountBlock = (TextBlock)FindName("CurrentOnline");
            var wordForms = new string[] { "игрок", "игрока", "игроков" };
            if (!onlineResponse.isOK) {
                onlineCountBlock.Text = "Не найдено";
            }
            else {
                try {
                    int playersCount = Int32.Parse(onlineResponse.data);
                    string wordForm = Globals.GetRightWordForm(playersCount, wordForms);
                    onlineCountBlock.Text = $"{playersCount} {wordForm}";
                }
                catch {
                    onlineCountBlock.Text = onlineResponse.data;
                }
            }
        }

        private void PlaySuccessLoadAnimation() {
            currentLoadingStep().Text = "";
            Storyboard fadeOutAnim = (Storyboard)FindResource("FadeOutModalBG");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
