using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Models
{
    public class SettingsModel : FileWithDefaultDictionaryContentModel
    {
        public Dictionary<string, string> SettingsAsDictionary = new Dictionary<string, string>();
        public SettingsModel() : base("launcher-config.ini", new Dictionary<string, string> {
                { "threads", "1" },
                { "custom_patches", "" }
        }) {
            SettingsAsDictionary = ReadAsDictionary();
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
