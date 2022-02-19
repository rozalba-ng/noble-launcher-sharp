using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Preloader.xaml
    /// </summary>
    public partial class Preloader : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        private UpdateServerAPIModel UpdateServerAPI;
        private readonly TextBlock CurrentLoadingStepText;

        public Preloader() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            CurrentLoadingStepText = (TextBlock)ElementSearcher.FindName("CurrentLoadingStep");
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartPreload, StartPreload);
        }

        private async void StartPreload() {
            var grid = (Grid)ElementSearcher.FindName("ModalBackground");
            UpdateServerAPI = UpdateServerAPIModel.Instance();
            grid.Opacity = 1;

            Migration();
            await CheckLauncherVersion();
            await GetBasePatches();
            PlaySuccessLoadAnimation();
        }

        public void Migration() {
            CurrentLoadingStepText.Text = "Мигрируем со старой версии";
            if (Directory.Exists(Settings.WORKING_DIR + "/Launcher")) {
                Directory.Delete(Settings.WORKING_DIR + "/Launcher", true);
            }

            if (File.Exists(Settings.WORKING_DIR + "/Noblegarden Launcher.exe")) {
                File.Delete(Settings.WORKING_DIR + "/Noblegarden Launcher.exe");
            }

        }

        public async Task CheckLauncherVersion() {
            if (UpdateServerAPI == null)
                return;
            CurrentLoadingStepText.Text = "Сверяем версии лаунчеров";
            var launcherVersionResponse = await UpdateServerAPI.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.FormattedData.version;
            if (actualLauncherVersion == "") {
                throw new Exception("Сервер не вернул актуальной версии лаунчера");
            }
            if (actualLauncherVersion != Settings.LAUNCHER_VERSION) {
                throw new Exception("Используется неактуальная версия лаунчера");
            }
        }
        private async Task GetBasePatches() {
            CurrentLoadingStepText.Text = "Получаем список патчей";
            var defaultPatchesResponse = await UpdateServerAPI.GetBasePatches();
            var patchesInfo = defaultPatchesResponse.FormattedData;
            var defaultPatches = JObjectConverter.ConvertToNecessaryPatchesList(patchesInfo);
            Static.Patches = new NoblePatchGroupModel<NecessaryPatchModel>(defaultPatches);
        }
        private void PlaySuccessLoadAnimation() {
            CurrentLoadingStepText.Text = "";
            var fadeOutAnim = ElementSearcher.FindStoryboard("FadeOutModalBG");
            if (fadeOutAnim != null) {
                fadeOutAnim.Begin();
            }
        }
    }
}
