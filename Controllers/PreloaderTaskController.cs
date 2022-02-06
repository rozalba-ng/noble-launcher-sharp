using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;

namespace NoblegardenLauncherSharp.Controllers
{
    public class PreloaderTaskController
    {
        private readonly SiteAPIModel SiteAPI = SiteAPIModel.Instance();
        private UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        private TextBlock CurrentLoadingStepText;
        private readonly ElementSearcherController ElementSearcher;
        public PreloaderTaskController () {
            ElementSearcher = new ElementSearcherController(App.Current.MainWindow);
            GetCurrentLoadingStepView();
        }

        public async Task CheckLauncherVersion() {
            //CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.CHECK_LAUNCHER_VERSION];
            var launcherVersionResponse = await UpdateServerAPI.GetActualLauncherVersion();
            string actualLauncherVersion = (string)launcherVersionResponse.GetFormattedData().version;
            if (actualLauncherVersion == "") {
                throw new Exception("Сервер не вернул актуальной версии лаунчера");
            }
            if (actualLauncherVersion != Globals.LAUNCHER_VERSION) {
                throw new Exception("Используется неактуальная версия лаунчера");
            }
        }

        public async Task DrawVisualInformation() {
            //CurrentLoadingStepText.Text = Globals.LOADING_TEXTS[(int)Globals.LOADING_STEPS.GET_LAST_UPDATES];
            await Task.WhenAll(
                Task.Run(() => GetAndDrawCustomPatches()),
                Task.Run(() => GetBasePatches())
            );
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
            Globals.Patches = new NoblePatchGroupModel<NecessaryPatchModel>(defaultPatches);
        }

        private async Task GetAndDrawCustomPatches() {
            var customPatchesResponse = await UpdateServerAPI.GetCustomPatches();
            var patchesInfo = customPatchesResponse.GetFormattedData();
            List<CustomPatchModel> customPatches = JObjectConverter.ConvertToCustomPatchesList(patchesInfo);
            Globals.CustomPatches = new NoblePatchGroupModel<CustomPatchModel>(customPatches);

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
                    //customPatchesView.ItemsSource = Globals.CustomPatches.List;
                })
            );
        }
        private TextBlock GetCurrentLoadingStepView() {
            if (CurrentLoadingStepText != null)
                return CurrentLoadingStepText;

            CurrentLoadingStepText = (TextBlock)ElementSearcher.FindName("CurrentLoadingStep");
            return CurrentLoadingStepText;
        }
    }
}
