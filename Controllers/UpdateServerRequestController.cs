using System;
using System.Threading.Tasks;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class UpdateServerRequestController : BaseRequestController
    {
        public UpdateServerRequestController(ServerModel Server) : base(Server) { }

        public async Task<NobleResponseModel> GetActualLauncherVersion() {
            var response = await MakeAsyncRequest($"{Server.URL}/launcher-version.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные об актуальной версии лаунчера");
            return response;
        }
        
        public async Task<NobleResponseModel> GetCustomPatches() {
            var response = await MakeAsyncRequest($"{Server.URL}/custom-patches.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные о необязательных патчах");
            return response;
        }

        public async Task<NobleResponseModel> GetBasePatches() {
            var response = await MakeAsyncRequest($"{Server.URL}/patches.json");
            if (!response.IsOK)
                throw new Exception("Не удалось получить данные об обязательных патчах");
            return response;
        }
    }
}
