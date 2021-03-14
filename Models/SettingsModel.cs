using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Models
{
    public class SettingsModel
    {
        public int ThreadsCount;
        public List<string> SelectedCustomPatches = new List<string>();

        public SettingsModel() {}

        public bool IsCustomPatchSelected(NoblePatchModel patch) {
            return false;
        }
    }
}
