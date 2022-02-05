using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    public class SettingsModel : FileWithDefaultDictionaryContentModel
    {
        private static SettingsModel instance;
        public Dictionary<string, string> SettingsAsDictionary = new Dictionary<string, string>();
        private SettingsModel() : base("launcher-config.ini", new Dictionary<string, string> {
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

        public void ToggleCustomPatchSavedSelection(string name) {
            var customPatches = GetSelectedCustomPatchesLocalPaths();
            if (customPatches.Contains(name)) {
                customPatches.Remove(name);
            } else {
                customPatches.Add(name);
            }

            SettingsAsDictionary["custom_patches"] = "";

            for (var i = 0; i < customPatches.Count; i++) {
                if (SettingsAsDictionary["custom_patches"].Length == 0) {
                    SettingsAsDictionary["custom_patches"] = customPatches[i];
                }
                else {
                    SettingsAsDictionary["custom_patches"] = SettingsAsDictionary["custom_patches"] + "," +customPatches[i];
                }
            }
            WriteDictionary(SettingsAsDictionary);
        }
    }
}
