using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp
{
    public class NoblegardenRequestController
    {
        private readonly string NOBLE_DOMAIN = "https://noblegarden.net";
        private string UPDATE_SERVER_ADDRESS;

        private async Task<NoblegardenRequestResponse> MakeAsyncRequest(string url) {
            string responseText = await Task.Run(() => {
                try {
                    WebRequest request = WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream responseStream = response.GetResponseStream();
                    return new StreamReader(responseStream).ReadToEnd();
                }
                catch (Exception e) {
                    Console.WriteLine("Error: " + e.Message);
                }

                return null;
            });

            if (responseText != null) {
                return new NoblegardenRequestResponse(true, responseText);
            }

            return new NoblegardenRequestResponse(false);
        }

        public async Task<NoblegardenRequestResponse> GetUpdateServerAddress() {
            var response = await MakeAsyncRequest($"{NOBLE_DOMAIN}/site/patches-ip");
            if (!response.isOK)
                return null;
            UPDATE_SERVER_ADDRESS = (string)response.data;

            return response;
        }
    }
}
