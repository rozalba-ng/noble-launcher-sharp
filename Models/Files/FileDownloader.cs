using NoblegardenLauncherSharp.Interfaces;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
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

        public static Task DownloadFile(IUpdateable patch, Action<long> onChunkLoaded) {
            if (CurrentWebClient != null) {
                throw new AccessViolationException("Web client уже существует");
            }

            if (File.Exists(patch.PathToTMP)) {
                File.Delete(patch.PathToTMP);
            }

            long previousDownloadedSize = 0;
            CurrentWebClient = new WebClient();

            void onProgressChanged(object sender, DownloadProgressChangedEventArgs e) {
                var diff = e.BytesReceived - previousDownloadedSize;
                previousDownloadedSize = e.BytesReceived;
                onChunkLoaded(diff);
            }

            void onComplete(object sender, AsyncCompletedEventArgs e) {
                CurrentWebClient = null;
            }

            CurrentWebClient.Proxy = WebRequest.DefaultWebProxy;
            CurrentWebClient.DownloadProgressChanged += onProgressChanged;
            CurrentWebClient.DownloadFileCompleted += onComplete;
            return CurrentWebClient.DownloadFileTaskAsync(new Uri(patch.RemotePath), patch.PathToTMP);
        }

        public static void AbortAnyLoad() {
            if (CurrentWebClient == null)
                return;

            CurrentWebClient.CancelAsync();
        }
    }
}
