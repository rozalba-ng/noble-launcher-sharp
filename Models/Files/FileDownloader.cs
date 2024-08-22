using NobleLauncher.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NobleLauncher.Models
{
    static class FileDownloader
    {
        private static WebClient CurrentWebClient;

        public static async Task<long> GetFileSize(IUpdateable patch) {
            long size = 0;
            var request = HttpWebRequest.CreateHttp(patch.RemotePath);
            request.Method = "HEAD";
            using (var response = await request.GetResponseAsync()) {
                size = response.ContentLength;
            }
            return size;
        }
        
        public static async Task<DateTime> GetLastModified(IUpdateable patch)
        {
            DateTime lastModified;
            var request = HttpWebRequest.CreateHttp(patch.RemotePath);
            request.Method = "HEAD";
            using (var response = await request.GetResponseAsync())
            {
                string lastModifiedString = response.Headers[HttpResponseHeader.LastModified];
                lastModified = DateTime.Parse(lastModifiedString);
            }

            return lastModified;
        }

        public static void CreateFolderForDownload(string fileDestination) {
            var dirPath = Path.GetDirectoryName(fileDestination);

            if (!Directory.Exists(dirPath)) {
                Directory.CreateDirectory(dirPath);
            }
        }

        public static Task DownloadFile(string from, string to, Action<long, int> onChunkLoaded) {
            if (CurrentWebClient != null) {
                throw new AccessViolationException("Web client уже существует");
            }

            CreateFolderForDownload(to);

            if (File.Exists(to)) {
                File.Delete(to);
            }

            long previousDownloadedSize = 0;
            CurrentWebClient = new WebClient();

            void onProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
                var diff = e.BytesReceived - previousDownloadedSize;
                previousDownloadedSize = e.BytesReceived;
                onChunkLoaded(diff, e.ProgressPercentage);
            }

            void onComplete(object sender, AsyncCompletedEventArgs e) {
                CurrentWebClient = null;
            }

            CurrentWebClient.Proxy = WebRequest.DefaultWebProxy;
            CurrentWebClient.DownloadProgressChanged += onProgressChanged;
            CurrentWebClient.DownloadFileCompleted += onComplete;
            return CurrentWebClient.DownloadFileTaskAsync(new Uri(from), to);
        }

        public static Task DownloadPatch(IUpdateable patch, Action<long, int> onChunkLoaded) {
            return DownloadFile(patch.RemotePath, patch.PathToTMP, onChunkLoaded);
        }

        public static void AbortAnyLoad() {
            if (CurrentWebClient == null)
                return;

            CurrentWebClient.CancelAsync();
        }

        public static Task DownloadFiles(List<IUpdateable> files,
            Action OnStart,
            Action<IUpdateable> OnNewFileDownload,
            Action<long, int> OnLoadUpdated)
        {
            if (files.Count == 0)
                return Task.Run(() => { });

            OnStart();

            return Task.Run(async () => {
                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    OnNewFileDownload(file);

                    await file.LoadUpdated(OnLoadUpdated);

                    if (!File.Exists(file.PathToTMP))
                        return;

                    if (File.Exists(file.FullPath))
                    {
                        File.Delete(file.FullPath);
                    }

                    File.Move(file.PathToTMP, file.FullPath);
                }
            });
        }
    }
}
