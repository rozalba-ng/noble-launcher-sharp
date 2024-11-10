using NobleLauncher.Globals;
using NobleLauncher.Interfaces;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace NobleLauncher.Components
{
    /// <summary>
    /// Логика взаимодействия для Updater.xaml
    /// </summary>
    public partial class Updater : UserControl
    {
        public Updater()
        {
            InitializeComponent();
            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompletePatchInfoRetrieving, FastCheckUpdateNeeded);
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, Update);
        }

        private List<IUpdateable> GetPatches()
        {
            List<IUpdateable> necessaryPatches = Static.Patches.List.ToList<IUpdateable>();
            List<IUpdateable> selectedCustomPatches = Static.CustomPatches.List.FindAll(patch => patch.Selected).ToList<IUpdateable>();

            List<IUpdateable> patches = new List<IUpdateable>();

            necessaryPatches.ForEach(patch => patches.Add(patch));
            selectedCustomPatches.ForEach(patch => patches.Add(patch));
            return patches;
        }

        private async void FastCheckUpdateNeeded()
        {
            var patches = GetPatches();
            await Task.Run(async () => {
                foreach (var patch in patches)
                {
                    DateTime lastModified = await patch.GetRemoteLastModified();
                    if (lastModified > patch.GetLastModified())
                    {
                        Static.InUIThread(() => {
                            ActionTextView.Text = "Некоторые патчи устарели, пришла пора обновиться.";
                        });
                        return;
                    }
                }
                Static.InUIThread(() => {
                    ActionTextView.Text = "Все патчи актуальны! Приятной игры!";
                });
            });
        }

        // https://stackoverflow.com/questions/876473/is-there-a-way-to-check-if-a-file-is-in-use
        protected virtual bool CanWriteToFile(string path)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            FileInfo file = new FileInfo(path);
            try
            {
                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return false;
            }
            return true;
        }

        private async Task UpdateLastModifiedTime(List<IUpdateable> patches)
        {
            foreach (IUpdateable patch in patches)
            {
                if (CanWriteToFile(patch.LocalPath))
                {
                    try
                    {
                        DateTime lastModified = await patch.GetRemoteLastModified();
                        File.SetLastWriteTime(patch.LocalPath, lastModified);
                    }
                    catch (Exception _)
                    {
                        continue;
                    }
                }
            }
        }

        public async void Update()
        {
            var patches = GetPatches();
            await CalcHashes(patches);
            await UpdateLastModifiedTime(patches);
            List<IUpdateable> patchesToUpdate = patches.FindAll(patch => patch.IsUpdateNeeded());

            await DownloadFiles(patchesToUpdate, await GetSummaryDownloadSize(patchesToUpdate));

            List<IUpdateable> addons = Static.Addons.List.FindAll(addon => addon.Selected && addon.IsUpdateNeeded()).ToList();
            await DownloadFiles(addons, await GetSummaryDownloadSize(addons), /*IsArchive=*/true);
            UpdateAddons(addons);
            
            CompleteUpdate();
        }

        private void UpdateAddons(List<IUpdateable> addons)
        {
            foreach (IUpdateable addon in addons)
            {
                var NameParts = addon.RemotePath.Split('/');
                string archive_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, addon.LocalPath, NameParts[NameParts.Length - 1]);
                ArchiveManager.ExtractToDirectoryWithOverwrite(archive_path, addon.LocalPath);
                File.Delete(archive_path);
            }
        }

        private long GetSummaryHashFileSize(List<IUpdateable> patches)
        {
            long summaryFileSize = 0;
            for (int i = 0; i < patches.Count; i++)
            {
                summaryFileSize += patches[i].GetPathByteSize();
            };

            return summaryFileSize;
        }
        private void CalcHash(IUpdateable patch)
        {
            if (patch.RemoteHash != null)
            {
                patch.LocalHash = patch.CalcCRC32Hash((blockSize) => { });
            }
        }

        private void UpdateProgressBar(int done, int total)
        {
            double progress = (double)(done) / (double)(total);
            int progressPercentage = (int)(progress * 100);
            Static.InUIThread(() => {
                ActionTextView.Text = "Считаем хэш-суммы... [" + done + "/" + total + "].";
            });
            SetProgress(progressPercentage);
        }

        private int hashesCounted = 0;

        private Task CalcHashes(List<IUpdateable> patches)
        {
            hashesCounted = 0;
            if (patches.Count == 0) return Task.Run(() => { });
            long summarySize = GetSummaryHashFileSize(patches);
            UpdateProgressBar(0, patches.Count);
            Task[] tasks = new Task[patches.Count];
            for (int i = 0; i < patches.Count; i++)
            {
                var patch = patches[i];
                tasks[i] = Task.Run(() => {
                    CalcHash(patch);
                    int newCount = Interlocked.Increment(ref hashesCounted);
                    UpdateProgressBar(newCount, patches.Count);
                });
            }

            return Task.Run(() => { Task.WhenAll(tasks).Wait(); });
        }

        private async Task<long> GetSummaryDownloadSize(List<IUpdateable> patches)
        {
            long summarySize = 0;
            if (patches.Count == 0)
            {
                return 0;
            }

            Static.InUIThread(() => {
                ActionTextView.Text = "Считаем размер обновления";
                SetProgress(0);
            });

            await Task.Run(async () => {
                for (int i = 0; i < patches.Count; i++)
                {
                    var patch = patches[i];
                    Static.InUIThread(() => {
                        ActionTextView.Text = "Считаем размер обновления: " + patch.LocalPath;
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

        private int downloadedPatchesCount;

        private Task DownloadFiles(List<IUpdateable> patches, long summarySize, bool IsArchive = false)
        {
            downloadedPatchesCount = 0;

            return FileDownloader.DownloadFiles(patches, () =>
                {
                    Static.InUIThread(() =>
                    {
                        ActionTextView.Text = "Загружаем файлы";
                        SetProgress(0);
                    });
                },
                (IUpdateable patch) =>
                {
                    downloadedPatchesCount++;
                    Static.InUIThread(() =>
                    {
                        ActionTextView.Text = "Загружаем файл: " + patch.Name + "(" + downloadedPatchesCount + "/" + patches.Count + ")";
                    });
                },
                (loadedChunkSize, percentOfFile) => SetProgress(percentOfFile),
                IsArchive);
        }

        private void CompleteUpdate()
        {
            Static.InUIThread(() => {
                ActionTextView.Text = "Обновлено!";
                ProgressTextView.Text = "";
                ProgressView.Value = 100;
            });

            EventDispatcher.Dispatch(EventDispatcherEvent.CompleteUpdate);
        }

        private void SetProgress(int progress)
        {
            Static.InUIThread(() => {
                if (progress != ProgressView.Value)
                {
                    ProgressView.Value = progress;
                    ProgressTextView.Text = progress.ToString() + "%";
                }
            });
        }
    }
}
