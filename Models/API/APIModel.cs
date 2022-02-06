using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    public class APIModel
    {
        public string BaseURL;

        public APIModel(string BaseURL) {
            this.BaseURL = BaseURL;
        }

        protected async Task<NobleResponseModel> MakeAsyncRequest(string TargetURL) {
            if (BaseURL == null) {
                return new NobleResponseModel(false, "Не определён адрес удаленного сервера");
            }

            string errMsg = "Неизвестная ошибка";
            Debug.WriteLine($"Request to: {BaseURL}{TargetURL}");

            string responseText = await Task.Run(() => {
                try {
                    WebRequest request = WebRequest.Create($"{BaseURL}{TargetURL}");
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
