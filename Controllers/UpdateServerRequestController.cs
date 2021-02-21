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
            if (!response.isOK)
                throw new Exception("Не удалось получить данные об актуальной версии лаунчера");
            return response;
        }
    }
}
