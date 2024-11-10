using Newtonsoft.Json.Linq;
using NobleLauncher.Globals;
using NobleLauncher.Interfaces;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace NobleLauncher.Components
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

        private bool clientFilesExist;
        private bool initialPatchesExist;
        private async void StartPreload()
        {
            UpdateServerAPI = UpdateServerAPIModel.Instance();
            ModalBackgroundView.Opacity = 1;
            Migration();
            await GetClientFilesList();
            await GetInitialPatchesList();
            clientFilesExist = FastCheckClientExists();
            initialPatchesExist = FastCheckPatchesExist();
            bool clientIsOk = clientFilesExist && initialPatchesExist;

            if (!clientIsOk)
            {
                ToggleLoadingAnimation(false);
                EnableDownloadButton();
            } else {
                await GetBasePatches();
                PlaySuccessLoadAnimation();
                EventDispatcher.Dispatch(EventDispatcherEvent.CompletePreload);
            }
        }

        private void EnableDownloadButton()
        {
            if (!CheckDirectoryName())
            {
                ShowError("Клиент не найден. Положите лаунчер в папку с именем Noblegarden (можно пустую).");
                return;
            }
            ToggleDownloadButton(true);
        }

        private void ToggleLoadingAnimation(bool toggle)
        {
            LoadingAnimation.IsHitTestVisible = toggle;
            LoadingAnimation.Visibility = toggle ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }
        private void ToggleDownloadButton(bool toggle) {
            DownloadClientButton.IsHitTestVisible = toggle;
            DownloadClientButton.Visibility = toggle ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            DownloadClientTextBlock.Visibility = toggle ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            CurrentLoadingStepView.Visibility = toggle ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
        }

        private void ShowError(string message)
        {
            Static.InUIThread(() => {
                CurrentLoadingStepView.Text = message;
            });
        }

        private bool CheckDirectoryName()
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string folderName = new DirectoryInfo(currentDirectory).Name;
            return folderName == "noblegarden" || folderName == "Noblegarden";
        }

        private bool FastCheckPatchesExist()
        {
            foreach (IUpdateable patch in Static.InitialPatches.List)
            {
                if (!File.Exists(patch.LocalPath))
                {
                    return false;
                }
            }
            return true;
        }
        private bool FastCheckClientExists()
        {
            foreach (FileModel file in Static.ClientFiles)
            {
                if (!file.Exists()) {
                    return false;
                }
            }
            return true;
        }

        public async void DownloadClient(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ToggleDownloadButton(false);
            // Needed because unzipping cannot replace files in .net framework.

            if (!clientFilesExist)
            {
                await DownloadClientFiles();
                ExtractClient();
            }
            if (!initialPatchesExist)
            {
                await DownloadInitialPatches();
            }
            EventDispatcher.Dispatch(EventDispatcherEvent.StartPreload);
        }

        private string client_archive_name = "noblegarden_client.zip";
        private Task DownloadClientFiles()
        {

            CurrentLoadingStepView.Text = "Скачивается архив с клиентом.";
            string client_link = "http://31.129.44.181/" + client_archive_name;
            string download_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, client_archive_name);
            
            return Task.Run(async () => {
                await FileDownloader.DownloadFileWithRetries(client_link, download_path, (chunkSize, percentage) =>
                    {
                        Static.InUIThread(() =>
                        {
                            CurrentLoadingProgressView.Value = percentage;
                        });
                    }
                );
            });
        }

        private void ExtractClient()
        {
            ArchiveManager.ExtractToDirectoryWithOverwrite(client_archive_name, AppDomain.CurrentDomain.BaseDirectory);
            File.Delete(client_archive_name);
        }
    
        private int patchIndex;
        public Task DownloadInitialPatches()
        {
            patchIndex = 0;
            List<IUpdateable> missingPatches = Static.InitialPatches.List
                .Where(patch => !File.Exists(patch.LocalPath))
                .ToList();
            return FileDownloader.DownloadFiles(missingPatches, () =>
                {
                    Static.InUIThread(() =>
                    {
                        CurrentLoadingStepView.Text = "Загружаем патчи.";
                        CurrentLoadingProgressView.Value = 0;
                    });
                },
                (IUpdateable patch) =>
                {
                    patchIndex++;
                    Static.InUIThread(() =>
                    {
                        CurrentLoadingStepView.Text = "Загрузка " + patch.LocalPath + "(" + patchIndex + "/" + missingPatches.Count + ")";
                    });
                },
                (loadedChunkSize, percentOfFile) =>
                {
                    Static.InUIThread(() =>
                    {
                        CurrentLoadingProgressView.Value = percentOfFile;
                    });
                });
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

        private async Task GetBasePatches() {
            CurrentLoadingStepView.Text = "Получаем список патчей";
            var defaultPatchesResponse = await UpdateServerAPI.GetBasePatches();
            var patchesInfo = defaultPatchesResponse.FormattedData;
            var defaultPatches = JObjectConverter.ConvertToNecessaryPatchesList(patchesInfo);
            Static.Patches = new NobleUpdatableGroupModel<IUpdateable>(defaultPatches);
        }

        private async Task GetInitialPatchesList()
        {
            CurrentLoadingStepView.Text = "Получаем список патчей.";
            var initialPatchesResponse = await UpdateServerAPI.GetInitialPatches();
            var patchesInfo = initialPatchesResponse.FormattedData;
            List<PatchModel> initialPatches = JObjectConverter.ConvertToPatchesList(patchesInfo);
            Static.InitialPatches = new NobleUpdatableGroupModel<IUpdateable>(initialPatches);
            CurrentLoadingStepView.Text = "Список патчей получен!";
        }

        private async Task GetClientFilesList()
        {
            CurrentLoadingStepView.Text = "Получаем список файлов клиента";
            NobleResponse ClientFilesResponse = await UpdateServerAPI.GetClientFiles();
            JObject filesInfo = ClientFilesResponse.FormattedData;
            Static.ClientFiles = JObjectConverter.ConvertToClientFileList(filesInfo);
            CurrentLoadingStepView.Text = "Cписок файлов клиента получен!";
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
