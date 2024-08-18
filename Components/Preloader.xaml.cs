using Newtonsoft.Json.Linq;
using NobleLauncher.Globals;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System;
using System.IO;
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
        private bool triedToDownloadClient;
        public Preloader() {
            InitializeComponent();
            triedToDownloadClient = false;
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartPreload, StartPreload);
        }

        private async void StartPreload()
        {
            UpdateServerAPI = UpdateServerAPIModel.Instance();
            ModalBackgroundView.Opacity = 1;
            Migration();

            await GetClientFilesList();
            bool clientIsOk = CheckIfClientFilesOk();
            if (!clientIsOk && triedToDownloadClient)
            {
                ShowError("Что-то пошло не так. Обратитесь к разработчикам программы за помощью.");
                return;
            }

            if (!clientIsOk)
            {
                ToggleLoadingAnimation(false);
            }
            if (clientIsOk)
            {
                await GetBasePatches();
                PlaySuccessLoadAnimation();
                EventDispatcher.Dispatch(EventDispatcherEvent.CompletePreload);
            }
        }

        private bool CheckIfClientFilesOk()
        {
            if (!FastCheckClientExists())
            {
                if (!CheckDirectoryName())
                {
                    ShowError("Клиент не найден. Положите лаунчер в папку с именем Noblegarden и перезапустите.");
                    return false;
                }
                ToggleDownloadButton(true);
                return false;
            }
            return true;
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

        private void RemoveClientFiles()
        {
            foreach (FileModel file in Static.ClientFiles)
            {
                if (file.Exists())
                {
                    file.Delete();
                }
            }
            string interface_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Interface");
            Directory.Delete(interface_path, true);
        }

        public async void DownloadClient(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RemoveClientFiles();
            string client_link = "http://31.129.44.181/noblegarden_client.zip";
            string download_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "noblegarden_client.zip");
            if (!File.Exists(download_path))
            {
                await FileDownloader.DownloadFile(client_link, download_path, (chunkSize, percentage) =>
                {
                    CurrentLoadingProgressView.Value = percentage;
                });
            }
            System.IO.Compression.ZipFile.ExtractToDirectory(download_path, AppDomain.CurrentDomain.BaseDirectory);
            File.Delete(download_path);
            triedToDownloadClient = true;
            EventDispatcher.Dispatch(EventDispatcherEvent.StartPreload);
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
            Static.Patches = new NoblePatchGroupModel<NecessaryPatchModel>(defaultPatches);
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
