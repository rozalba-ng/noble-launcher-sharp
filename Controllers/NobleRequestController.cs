using System;
using System.Threading.Tasks;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class NobleRequestController : BaseRequestController {
        public NobleRequestController(ServerModel Server) : base(Server) { }

        public async Task<NobleResponseModel> GetUpdateServerAddress() {
            var response = await MakeAsyncRequest($"{Server.URL}/site/patches-ip");
            if (!response.isOK) {
                throw new Exception("Не удалось получить адрес сервера обновлений");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetOnlineCount() {
            return await MakeAsyncRequest($"{Server.URL}/armory/online");
        }

        public async Task<NobleResponseModel> GetLastNews() {
            return await MakeAsyncRequest($"{Server.URL}/site/articles");
        }
    }
}
