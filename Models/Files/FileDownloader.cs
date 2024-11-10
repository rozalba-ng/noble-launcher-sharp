using NobleLauncher.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NobleLauncher.Models
{
    public class WebClientRange : WebClient
    {
        private readonly long from;

        public WebClientRange(long from)
        {
            this.from = from;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.AddRange(this.from);
            return request;
        }
    }

    static class FileDownloader
    {
        private static WebClientRange CurrentWebClient;

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

        public static Task DownloadPatch(IUpdateable patch, Action<long, int> onChunkLoaded)
        {
            return DownloadFileWithRetries(patch.RemotePath, patch.PathToTMP, onChunkLoaded);
        }

        public static Task DownloadFileWithRetries(string from, string to, Action<long, int> onChunkLoaded, int maxRetries = 30, int retryDelay = 5000)
        {
            return Task.Run(async () => {
                int attempt = 0;
                bool downloadComplete = false;

                while (attempt < maxRetries && !downloadComplete)
                {
                    Console.WriteLine("Attempt: " + attempt.ToString());
                    try
                    {
                        // Call the updated method that supports partial downloads
                        await DownloadFileWithResume(from, to, onChunkLoaded);
                        downloadComplete = true; // Success
                    }
                    catch (WebException ex)
                    {
                        attempt++;
                        if (attempt >= maxRetries)
                        {
                            throw new Exception($"Download failed after {maxRetries} attempts: {ex.Message}", ex);
                        }
                        // Wait before retrying
                        await Task.Delay(retryDelay);
                    }
                }
            });
        }

        public static Task DownloadFileWithResume(string from, string to, Action<long, int> onChunkLoaded)
        {
            CreateFolderForDownload(to);

            long existingFileSize = 0;
            if (File.Exists(to))
            {
                existingFileSize = new FileInfo(to).Length;
            }

            long previousDownloadedSize = existingFileSize;
            long totalDownloadedSize = existingFileSize;
            long totalFileSize = 0;

            CurrentWebClient = new WebClientRange(existingFileSize);

            CurrentWebClient.Proxy = WebRequest.DefaultWebProxy;

            // Download file directly to the target file and append as chunks arrive
            return Task.Run(async () =>
            {
                // Open the file in append mode, so new data is added after the existing content
                using (var fileStream = new FileStream(to, FileMode.Append, FileAccess.Write, FileShare.None))
                {
                    using (var webStream = await CurrentWebClient.OpenReadTaskAsync(new Uri(from)))
                    {
                        totalFileSize = long.Parse(CurrentWebClient.ResponseHeaders["Content-Length"]) + existingFileSize;

                        byte[] buffer = new byte[8192]; // Buffer size
                        int bytesRead;

                        while ((bytesRead = await webStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalDownloadedSize += bytesRead;

                            // Calculate progress and call the progress callback
                            int progressPercentage = (int)((double)totalDownloadedSize / totalFileSize * 100);
                            onChunkLoaded(bytesRead, progressPercentage);
                        }
                    }
                }
            });
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
