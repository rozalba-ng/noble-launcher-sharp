using NoblegardenLauncherSharp.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Controllers
{
    public class BaseRequestController
    {
        protected readonly ServerModel Server;

        public BaseRequestController(ServerModel Server) {
            this.Server = Server;
        }

        protected async Task<NobleResponseModel> MakeAsyncRequest(string url) {
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
                return new NobleResponseModel(responseText);
            }

            return new NobleResponseModel(false, $"Не удалось соединиться с сервером. ({errMsg})");
        }
    }
}
