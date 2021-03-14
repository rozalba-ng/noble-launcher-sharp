using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Controllers
{
    public class SettingsFileController : BaseFileController
    {
        public Dictionary<string, string> SettingsAsDictionary = new Dictionary<string, string>();
        public SettingsFileController() : base("launcher-config.ini") {
            SettingsAsDictionary = ReadAsDictionary();
        }

        protected override Dictionary<string, string> GetDefaultContent() {
            var defaultContent = new Dictionary<string, string> {
                { "threads", "1" },
                { "custom_patches", "" }
            };
            return defaultContent;
        }
    }
}
