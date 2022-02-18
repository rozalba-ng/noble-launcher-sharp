using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Interfaces;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shell;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Updater.xaml
    /// </summary>
    public partial class Updater : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        private readonly ProgressBar ProgressBar;
        private readonly TextBlock CurrentAction;
        private readonly TextBlock CurrentProgress;

        public Updater() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            ProgressBar = (ProgressBar)ElementSearcher.FindName("Progress");
            CurrentAction = (TextBlock)ElementSearcher.FindName("CurrentActionText");
            CurrentProgress = (TextBlock)ElementSearcher.FindName("CurrentActionProgress");
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, Update);
        }
        public async void Update() {
            List<IUpdateable> necessaryPatches = Static.Patches.List.ToList<IUpdateable>();
            List<IUpdateable> selectedCustomPatches = Static.CustomPatches.List.FindAll(patch => patch.Selected).ToList<IUpdateable>();

            List<IUpdateable> patches = new List<IUpdateable>();
            necessaryPatches.ForEach(patch => patches.Add(patch));
            selectedCustomPatches.ForEach(patch => patches.Add(patch));

            await CalcHashes(patches);

            List<IUpdateable> patchesToUpdate = patches.FindAll(patch => patch.LocalHash != patch.RemoteHash);

            await DownloadFiles(patchesToUpdate, await GetSummaryDownloadSize(patchesToUpdate));
            RenameFiles(patchesToUpdate);
            CompleteUpdate();
        }

        private long GetSummaryHashFileSize(List<IUpdateable> patches) {
            long summaryFileSize = 0;
            for (int i = 0; i < patches.Count; i++) {
                summaryFileSize += patches[i].GetPathByteSize();
            };

            return summaryFileSize;
        }

        private Task CalcHashes(List<IUpdateable> patches) {
            if (patches.Count == 0) return Task.Run(() => { });
            long currentRead = 0;
            long summarySize = GetSummaryHashFileSize(patches);

            return Task.Run(async () => {
                for (int i = 0; i < patches.Count; i++) {
                    var patch = patches[i];
                    Static.InUIThread(() => {
                        CurrentAction.Text = "Считаем чек-суммы: " + patch.LocalPath;
                    });
                    var hash = await patch.CalcCRC32Hash((blockSize) => {
                        currentRead += blockSize;
                        double readBytesProgress = (double)(currentRead) / (double)(summarySize);
                        int progress = (int)(readBytesProgress * 100);
                        SetProgress(progress);
                    });

                    patch.LocalHash = hash;
                }
            });
        }

        private async Task<long> GetSummaryDownloadSize(List<IUpdateable> patches) {
            long summarySize = 0;
            if (patches.Count == 0) {
                return 0;
            }

            Static.InUIThread(() => {
                CurrentAction.Text = "Считаем размер обновления";
                SetProgress(0);
            });
            
            await Task.Run(async () => {
                for (int i = 0; i < patches.Count; i++) {
                    var patch = patches[i];
                    Static.InUIThread(() => {
                        CurrentAction.Text = "Считаем размер обновления: " + patch.LocalPath;
                    });

                    var size = await patch.GetRemoteSize();
                    summarySize += size;
                    double fileProgress = (double)(i + 1) / (double)(patches.Count);
                    int progress = (int)(fileProgress * 100);
                    SetProgress(progress);
                }
            });

            return summarySize;
        }

        private Task DownloadFiles(List<IUpdateable> patches, long summarySize) {
            if (patches.Count == 0)
                return Task.Run(() => { });
            long currentLoaded = 0;

            Static.InUIThread(() => {
                CurrentAction.Text = "Загружаем файлы";
                SetProgress(0);
            });

            return Task.Run(async () => {
                for (int i = 0; i < patches.Count; i++) {
                    var patch = patches[i];
                    Static.InUIThread(() => {
                        CurrentAction.Text = "Загружаем файл: " + patch.LocalPath + "(" + (i + 1) + "/" + patches.Count + ")";
                    });

                    await patch.LoadUpdated((loadedChunkSize) => {
                        currentLoaded += loadedChunkSize;
                        double downloadProgress = (double)(currentLoaded) / (double)(summarySize);
                        int progress = (int)(downloadProgress * 100);
                        SetProgress(progress);
                    });
                }
            });
        }

        private void RenameFiles(List<IUpdateable> patches) {
            if (patches.Count == 0)
                return;
            patches.ForEach(patch => {
                if (!File.Exists(patch.PathToTMP))
                    return;

                if (File.Exists(patch.FullPath)) {
                    File.Delete(patch.FullPath);
                }

                File.Move(patch.PathToTMP, patch.FullPath);
            });
        }

        private void CompleteUpdate() {
            Static.InUIThread(() => {
                CurrentAction.Text = "Обновлено!";
                CurrentProgress.Text = "";
                ProgressBar.Value = 100;
            });

            EventDispatcher.Dispatch(EventDispatcherEvent.CompleteUpdate);
        }

        private void SetProgress(int progress) {
            Static.InUIThread(() => {
                if (progress != ProgressBar.Value) {
                    ProgressBar.Value = progress;
                    CurrentProgress.Text = progress.ToString() + "%";
                }
            });
        }
    }
}
