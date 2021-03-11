using System.IO;

namespace NoblegardenLauncherSharp.Controllers
{
    public class SettingsFileController : BaseFileController
    {
        public SettingsFileController() : base("launcher-config.ini") {}

        protected override string[] GetDefaultContent() {
            return new string[] {
                "threads=1",
                "custom_patches="
            };
        }
    }
}
