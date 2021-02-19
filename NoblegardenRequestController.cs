using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp
{
    public class NoblegardenRequestController
    {
        private string UPDATE_SERVER_ADDRESS;

        private async Task<NoblegardenRequestResponse> MakeAsyncRequest(string url) {
            string errMsg = "Неизвестная ошибка";
            Debug.WriteLine($"Request to: {url}");

            string responseText = await Task.Run(() => {
                try {
                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream).ReadToEnd();
                }
                catch (Exception e) {
                    errMsg = $"Error: {e.Message}";
                }

                return null;
            });

            if (responseText != null) {
                return new NoblegardenRequestResponse(responseText);
            }

            return new NoblegardenRequestResponse(false, $"Не удалось соединиться с сервером. ({errMsg})");
        }

        public async Task<NoblegardenRequestResponse> GetUpdateServerAddress() {
            var response = await MakeAsyncRequest($"{Globals.NOBLE_DOMAIN}/site/patches-ip");
            if (response.isOK) {
                UPDATE_SERVER_ADDRESS = response.data;
            }
            return response;
        }

        public async Task<NoblegardenRequestResponse> GetActualLauncherVersion() {
            if (UPDATE_SERVER_ADDRESS == null)
                return new NoblegardenRequestResponse(false, "Не удалось получить актуальную версию лаунчера");

            var response = await MakeAsyncRequest($"http://{UPDATE_SERVER_ADDRESS}/launcher-version.json");
            return response;
        }

        public async Task<NoblegardenRequestResponse> GetOnlineCount() {
            return await MakeAsyncRequest($"{Globals.NOBLE_DOMAIN}/armory/online");
        }

        public async Task<NoblegardenRequestResponse> GetLastNews() {
            return await MakeAsyncRequest($"{Globals.NOBLE_DOMAIN}/site/articles");
        }
    }
}
