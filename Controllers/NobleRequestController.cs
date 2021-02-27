using System;
using System.Threading.Tasks;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class NobleRequestController : BaseRequestController {
        public NobleRequestController(ServerModel Server) : base(Server) { }

        public async Task<NobleResponseModel> GetUpdateServerAddress() {
            var response = await MakeAsyncRequest($"{Server.URL}/site/patches-ip");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить адрес сервера обновлений");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetOnlineCount() {
            var response = await MakeAsyncRequest($"{Server.URL}/armory/online");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить текущий онлайн");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetLastNews() {
            var response = await MakeAsyncRequest($"{Server.URL}/site/articles");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить новости");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetActualDiscordLink() {
            var response = await MakeAsyncRequest($"{Server.URL}/site/discord-link");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить ссылку на Discord");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetActualVKLink() {
            var response = await MakeAsyncRequest($"{Server.URL}/site/vk-link");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить ссылку на VK");
            }
            return response;
        }
    }
}
