using System;
using System.Collections.Generic;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Globals
{
    public static class Settings {
        public static readonly string WORKING_DIR = @"D:\Games\Noblegarden";
        public static readonly string NOBLE_DOMAIN = "https://noblegarden.net";
        public static readonly string LAUNCHER_VERSION = "1.3.2";

        private static List<string> SELECTED_CUSTOM_PATCHES = new List<string>();

        private static readonly FileWithDefaultDictionaryContentModel SettingsFile = new FileWithDefaultDictionaryContentModel(
            "launcher-config.ini",
            new Dictionary<string, string> {
                { "custom_patches", "" }
            }
        );

        private static Dictionary<string, string> SettingsRepresentation = new Dictionary<string, string>();

        public static void Parse() {
            var config = SettingsFile.ReadAsDictionary();
            SettingsRepresentation = config;

            ParseSelectedCustomPatches(SettingsRepresentation);
        }

        public static List<string> GetSelectedCustomPatches() {
            return SELECTED_CUSTOM_PATCHES;
        }

        private static void ParseSelectedCustomPatches(Dictionary<string, string> config) {
            if (!config.ContainsKey("custom_patches"))
                return;

            var patches = config["custom_patches"];

            if (patches == null || patches.Length == 0) {
                SELECTED_CUSTOM_PATCHES = new List<string>();
                return;
            }

            var splittedPatches = patches.Split(',');
            SELECTED_CUSTOM_PATCHES = new List<string>(splittedPatches);
        }

        public static void ToggleCustomPatchSelection(string name) {
            if (SELECTED_CUSTOM_PATCHES.Contains(name)) {
                SELECTED_CUSTOM_PATCHES.Remove(name);
            }
            else {
                SELECTED_CUSTOM_PATCHES.Add(name);
            }

            SettingsRepresentation["custom_patches"] = "";

            for (var i = 0; i < SELECTED_CUSTOM_PATCHES.Count; i++) {
                if (SettingsRepresentation["custom_patches"].Length == 0) {
                    SettingsRepresentation["custom_patches"] = SELECTED_CUSTOM_PATCHES[i];
                }
                else {
                    SettingsRepresentation["custom_patches"] += "," + SELECTED_CUSTOM_PATCHES[i];
                }
            }

            SettingsFile.WriteDictionary(SettingsRepresentation);
        }
    }
}
