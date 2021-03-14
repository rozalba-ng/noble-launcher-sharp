using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Models
{
    public class SettingsModel : FileWithDefaultDictionaryContentModel
    {
        private static SettingsModel instance;
        public Dictionary<string, string> SettingsAsDictionary = new Dictionary<string, string>();
        private SettingsModel() : base("launcher-config.ini", new Dictionary<string, string> {
                { "threads", "1" },
                { "custom_patches", "" }
        }) {
            SettingsAsDictionary = ReadAsDictionary();
        }

        public static SettingsModel Init() {
            if (instance == null) {
                instance = new SettingsModel();
            }

            return instance;
        }

        public static SettingsModel GetInstance() {
            return instance;
        }

        public List<string> GetSelectedCustomPatchesLocalPaths() {
            if (SettingsAsDictionary.Count == 0)
                return new List<string>();

            var customPatchesAsString = SettingsAsDictionary["custom_patches"];
            if (customPatchesAsString == null || customPatchesAsString.Length == 0)
                return new List<string>();

            var splittedPatches = customPatchesAsString.Split(',');
            return new List<string>(splittedPatches);
        }
    }
}
