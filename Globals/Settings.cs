using System;
using System.Collections.Generic;
using NobleLauncher.Models;

namespace NobleLauncher.Globals
{
    public static class Settings {
        public static readonly string WORKING_DIR = @".";
        public static readonly string NOBLE_DOMAIN = "https://noblegarden.net";
        public static readonly string LAUNCHER_VERSION = "2.0.5";
        public static bool ENABLE_TLS = false;
        public static bool ENABLE_DEBUG_MODE = false;

        private static List<string> SELECTED_CUSTOM_PATCHES = new List<string>();
        private static List<string> SELECTED_ADDONS = new List<string>();

        private static readonly FileWithDefaultDictionaryContentModel SettingsFile = new FileWithDefaultDictionaryContentModel(
            "launcher-config.ini",
            new Dictionary<string, string> {
                { "custom_patches", "" },
                { "addons", "" },
                { "enable_tls", "false" },
                { "debug_mode", "false" }
            }
        );

        private static Dictionary<string, string> SettingsRepresentation = new Dictionary<string, string>();

        public static void Parse() {
            var config = SettingsFile.ReadAsDictionary();
            SettingsRepresentation = config;

            ParseSelectedCustomPatches(SettingsRepresentation);
            ParseSelectedAddons(SettingsRepresentation);
            ParseEnableTLS(SettingsRepresentation);
            ParseEnableDebugMode(SettingsRepresentation);
        }

        public static List<string> GetSelectedCustomPatches() {
            return SELECTED_CUSTOM_PATCHES;
        }
        public static List<string> GetSelectedAddons()
        {
            return SELECTED_ADDONS;
        }

        private static void Toggle(string name, List<string> source, string category)
        {
            if (source.Contains(name))
            {
                source.Remove(name);
            }
            else
            {
                source.Add(name);
            }

            SettingsRepresentation[category] = string.Join(",", source);

            SettingsFile.WriteDictionary(SettingsRepresentation);
        }

        public static void ToggleCustomPatchSelection(string name) {
            Toggle(name, SELECTED_CUSTOM_PATCHES, "custom_patches");
        }
        public static void ToggleAddonSelection(string name)
        {
            Toggle(name, SELECTED_ADDONS, "addons");
        }

        private static void ParseSelected(Dictionary<string, string> config, List<string> destination, string category)
        {
            if (!config.TryGetValue(category, out var saved) || string.IsNullOrEmpty(saved))
            {
                destination.Clear();
            }
            else
            {
                destination.Clear();
                destination.AddRange(saved.Split(','));
            }
        }

        private static void ParseSelectedCustomPatches(Dictionary<string, string> config) {
            ParseSelected(config, SELECTED_CUSTOM_PATCHES, "custom_patches");
        }
        private static void ParseSelectedAddons(Dictionary<string, string> config)
        {
            ParseSelected(config, SELECTED_ADDONS, "addons");
        }

        private static void ParseEnableTLS(Dictionary<string, string> config) {
            ENABLE_TLS = ParseAndReturnBoolVal(config, "enable_tls");
        }

        private static void ParseEnableDebugMode(Dictionary<string, string> config) {
            ENABLE_DEBUG_MODE = ParseAndReturnBoolVal(config, "debug_mode");
        }

        private static bool ParseAndReturnBoolVal(Dictionary<string, string> config, string key) {
            if (!config.ContainsKey(key)) {
                config.Add(key, "false");
                SettingsFile.WriteDictionary(config);
                return false;
            }

            return config[key] == "true";
        }
    }
}
