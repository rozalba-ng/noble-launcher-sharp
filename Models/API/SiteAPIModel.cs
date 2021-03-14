using System;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    public class SiteAPIModel : APIModel
    {
        private static SiteAPIModel instance;
        private SiteAPIModel(string BaseURL) : base(BaseURL) { }

        public static SiteAPIModel Init(string BaseURL) {
            if (instance == null) {
                instance = new SiteAPIModel(BaseURL);
            }

            return instance;
        }

        public static SiteAPIModel GetInstance() {
            return instance;
        }
        public async Task<NobleResponseModel> GetUpdateServerAddress() {
            var response = await MakeAsyncRequest("/site/patches-ip");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить адрес сервера обновлений");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetOnlineCount() {
            var response = await MakeAsyncRequest($"/armory/online");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить текущий онлайн");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetLastNews() {
            var response = await MakeAsyncRequest($"/site/articles");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить новости");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetActualDiscordLink() {
            var response = await MakeAsyncRequest($"/site/discord-link");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить ссылку на Discord");
            }
            return response;
        }

        public async Task<NobleResponseModel> GetActualVKLink() {
            var response = await MakeAsyncRequest($"/site/vk-link");
            if (!response.IsOK) {
                throw new Exception("Не удалось получить ссылку на VK");
            }
            return response;
        }
    }
}
