using System.Collections.Generic;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp
{
    public static class Globals
    {
        public static readonly string NOBLE_DOMAIN = "https://noblegarden.net";
        public static readonly string LAUNCHER_VERSION = "1.3.1";
        public static readonly List<SliderElementModel> SliderElements = new List<SliderElementModel> {
            new SliderElementModel("Персонажи", "https://noblegarden.net/charlist", "Images/square-character.jpg"),
            new SliderElementModel("Гильдии", "https://noblegarden.net/guild", "Images/square-guild.jpg"),
            new SliderElementModel("Сюжеты", "https://noblegarden.net/storyline", "Images/square-plots.jpg")
        };

        public enum LOADING_STEPS {
            GET_SERVER_ADDRESS = 0,
            CHECK_LAUNCHER_VERSION = 1,
            GET_LAST_UPDATES = 2,
        }

        public static readonly string[] LOADING_TEXTS = {
            "Получаем адрес сервера обновлений",
            "Сверяем версии лаунчеров",
            "Получаем последние новости",
        };

        public static string GetRightWordForm(int count, string[] words) {
            if (count <= 0)
                return words[2];

            if (count < 20) {
                if (count == 1)
                    return words[0];
                if (count < 5)
                    return words[1];
                return words[2];
            }

            if (count < 100) {
                int lastNumberInCount = count % 10;
                return GetRightWordForm(lastNumberInCount, words);
            }

            int lastTenInCount = count % 100;
            return GetRightWordForm(lastTenInCount, words);
        }
    }
}
