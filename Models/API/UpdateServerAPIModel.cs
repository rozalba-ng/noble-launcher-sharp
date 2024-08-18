using System.Threading.Tasks;
using NobleLauncher.Globals;
using NobleLauncher.Structures;

namespace NobleLauncher.Models
{
    public class UpdateServerAPIModel : APIModel
    {
        private static UpdateServerAPIModel instance;
        private UpdateServerAPIModel(string BaseURL) : base(BaseURL) { }

        public static UpdateServerAPIModel Instance() {
            if (instance == null) {
                SiteAPIModel SiteAPI = SiteAPIModel.Instance();
                var updateServerAddressResponse = Task.Run(() => SiteAPI.GetUpdateServerAddress()).Result;
                string updateServerIP = (string)updateServerAddressResponse.FormattedData;
                instance = new UpdateServerAPIModel($"http://{updateServerIP}");
            }

            return instance;
        }

        public async Task<NobleResponse> GetActualLauncherVersion() {
            var response = await MakeAsyncRequest("/launcher-version.json");
            if (!response.IsOK) {
                Static.ShutdownWithError("Не удалось получить данные об актуальной версии лаунчера");
                return new NobleResponse();
            }
            return response;
        }

        public async Task<NobleResponse> GetCustomPatches() {
            var response = await MakeAsyncRequest("/custom-patches.json");
            if (!response.IsOK) {
                Static.ShutdownWithError("Не удалось получить данные о необязательных патчах");
                return new NobleResponse();
            }
            return response;
        }

        public async Task<NobleResponse> GetBasePatches() {
            var response = await MakeAsyncRequest("/patches.json");
            if (!response.IsOK) {
                Static.ShutdownWithError("Не удалось получить данные об обязательных патчах");
                return new NobleResponse();
            }
            return response;
        }
        public async Task<NobleResponse> GetClientFiles()
        {
            var response = await MakeAsyncRequest("/client_files.json");
            if (!response.IsOK)
            {
                Static.ShutdownWithError("Не удалось получить данные о необходимых для клиента файлах.");
                return new NobleResponse();
            }
            return response;
        }
    }
}
