using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Preloader.xaml
    /// </summary>
    public partial class Preloader : UserControl
    {
        private UpdateServerAPIModel UpdateServerAPI;

        public Preloader() {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartPreload, StartPreload);
        }

        private async void StartPreload() {
            UpdateServerAPI = UpdateServerAPIModel.Instance();
            ModalBackgroundView.Opacity = 1;

            Migration();
            await CheckLauncherVersion();
            await GetBasePatches();
            PlaySuccessLoadAnimation();
        }

        public void Migration() {
            CurrentLoadingStepView.Text = "Мигрируем со старой версии";
            if (Directory.Exists(Settings.WORKING_DIR + "/Launcher")) {
                Directory.Delete(Settings.WORKING_DIR + "/Launcher", true);
            }

            if (File.Exists(Settings.WORKING_DIR + "/Noblegarden Launcher.exe")) {
                File.Delete(Settings.WORKING_DIR + "/Noblegarden Launcher.exe");
            }
        }

        private async Task DownloadNewLauncherVersion(string linkToLauncher) {
            CurrentLoadingStepView.Text = "Скачиваем новую версию лаунчера";
            CurrentLoadingProgressView.Opacity = 1;
            await FileDownloader.DownloadFile(
                linkToLauncher,
                Settings.WORKING_DIR + "/Noble Launcher.exe.tmp",
                (chunkSize, percentage) => {
                    CurrentLoadingProgressView.Value = percentage;
                }
            );
            CurrentLoadingProgressView.Opacity = 0;
        }

        public async Task CheckLauncherVersion() {
            if (UpdateServerAPI == null)
                return;
            CurrentLoadingStepView.Text = "Сверяем версии лаунчеров";
            var launcherVersionResponse = await UpdateServerAPI.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.FormattedData.version;
            if (actualLauncherVersion == "") {
                Static.ShutdownWithError("Сервер не вернул актуальной версии лаунчера");
            }
            if (actualLauncherVersion != Settings.LAUNCHER_VERSION) {
                await DownloadNewLauncherVersion((string)launcherVersionResponse.FormattedData.link);
                Static.ShutdownWithError("Используется неактуальная версия лаунчера.\nТекущая: " + Settings.LAUNCHER_VERSION + ", актуальная: " + actualLauncherVersion);
            }
        }
        private async Task GetBasePatches() {
            CurrentLoadingStepView.Text = "Получаем список патчей";
            var defaultPatchesResponse = await UpdateServerAPI.GetBasePatches();
            var patchesInfo = defaultPatchesResponse.FormattedData;
            var defaultPatches = JObjectConverter.ConvertToNecessaryPatchesList(patchesInfo);
            Static.Patches = new NoblePatchGroupModel<NecessaryPatchModel>(defaultPatches);
        }
        private void PlaySuccessLoadAnimation() {
            CurrentLoadingStepView.Text = "";
            var fadeOutAnim = (Storyboard)FindResource("FadeOutModalBG");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
