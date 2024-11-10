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
        private async Task<NobleResponse> GetDataAsync(string url, string errorMessage)
        {
            var response = await MakeAsyncRequest(url);
            if (!response.IsOK)
            {
                Static.ShutdownWithError(errorMessage);
                return new NobleResponse();
            }
            return response;
        }

        public Task<NobleResponse> GetActualLauncherVersion()
        {
            return GetDataAsync("/launcher-version.json", "Не удалось получить данные об актуальной версии лаунчера");
        }

        public Task<NobleResponse> GetCustomPatches()
        {
            return GetDataAsync("/custom-patches.json", "Не удалось получить данные о необязательных патчах.");
        }

        public Task<NobleResponse> GetBasePatches()
        {
            return GetDataAsync("/patches.json", "Не удалось получить данные об обязательных патчах.");
        }

        public Task<NobleResponse> GetAddons()
        {
            return GetDataAsync("/addons.json", "Не удалось получить данные об аддонах.");
        }

        public Task<NobleResponse> GetInitialPatches()
        {
            return GetDataAsync("/initial_patches.json", "Не удалось получить данные о базовых патчах.");
        }

        public Task<NobleResponse> GetClientFiles()
        {
            return GetDataAsync("/client_files.json", "Не удалось получить данные о необходимых для клиента файлах.");
        }
    }
}
