using System;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    public class UpdateServerAPIModel : APIModel
    {
        public UpdateServerAPIModel() : base() { }

        public async Task<NobleResponseModel> GetActualLauncherVersion() {
            var response = await MakeAsyncRequest("/launcher-version.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные об актуальной версии лаунчера");
            return response;
        }

        public async Task<NobleResponseModel> GetCustomPatches() {
            var response = await MakeAsyncRequest("/custom-patches.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные о необязательных патчах");
            return response;
        }

        public async Task<NobleResponseModel> GetBasePatches() {
            var response = await MakeAsyncRequest("/patches.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные об обязательных патчах");
            return response;
        }
    }
}
