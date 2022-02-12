using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class PreloaderTaskController
    {
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        private readonly UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        private TextBlock CurrentLoadingStepText;
        private readonly ElementSearcher ElementSearcher;
        public PreloaderTaskController () {
            ElementSearcher = new ElementSearcher(App.Current.MainWindow);
            GetCurrentLoadingStepView();
        }

        public async Task CheckLauncherVersion() {
            //CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.CHECK_LAUNCHER_VERSION];
            var launcherVersionResponse = await UpdateServerAPI.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.GetFormattedData().version;
            if (actualLauncherVersion == "") {
                throw new Exception("Сервер не вернул актуальной версии лаунчера");
            }
            if (actualLauncherVersion != Settings.LAUNCHER_VERSION) {
                throw new Exception("Используется неактуальная версия лаунчера");
            }
        }

        public async Task DrawVisualInformation() {
            //CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_LAST_UPDATES];
            await Task.Run(() => GetBasePatches());
        }

        public void PlaySuccessLoadAnimation() {
            //CurrentLoadingStepText.Text = "";
            var fadeOutAnim = ElementSearcher.FindStoryboard("FadeOutModalBG");
            if (fadeOutAnim != null) {
                //fadeOutAnim.Begin();
            }
        }

        private async Task GetBasePatches() {
            var defaultPatchesResponse = await UpdateServerAPI.GetBasePatches();
            var patchesInfo = defaultPatchesResponse.GetFormattedData();
            var defaultPatches = JObjectConverter.ConvertToNecessaryPatchesList(patchesInfo);
            Static.Patches = new NoblePatchGroupModel<NecessaryPatchModel>(defaultPatches);
        }

        private TextBlock GetCurrentLoadingStepView() {
            if (CurrentLoadingStepText != null)
                return CurrentLoadingStepText;

            CurrentLoadingStepText = (TextBlock)ElementSearcher.FindName("CurrentLoadingStep");
            return CurrentLoadingStepText;
        }
    }
}
