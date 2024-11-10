using NobleLauncher.Globals;
using System;
using System.Threading.Tasks;
using System.IO;


namespace NobleLauncher.Models
{
    public class AddonModel : PatchModel
    {
        public Version RemoteVersion;

        public override string FullPath
        {
            get => Settings.WORKING_DIR + "/" + LocalPath + "/" + Name;
        }

        public override string PathToTMP
        {
            get => FullPath + ".tmp";
        }

        public AddonModel() : base()
        {
            ChangeSelectionTo(false);
        }
        public AddonModel(string LocalPath, string RemotePath, string Hash, string Description) : base(LocalPath, RemotePath, Hash, Description) {
            ChangeSelectionTo(false);
            GetSelectionFromSettings();
        }

        public override void ChangeSelectionTo(bool To) {
            Selected = To;
        }

        public void GetSelectionFromSettings() {
            var selectAddonNames = Settings.GetSelectedAddons();
            Parallel.For(0, selectAddonNames.Count, (i) => {
                if (selectAddonNames[i] == Name) {
                    ChangeSelectionTo(true);
                }
            });
        }

        public void CalcLocalPath()
        {
            LocalPath = Path.Combine("Interface", "AddOns");
        }

        public override bool IsUpdateNeeded()
        {
            try
            {
                string versionFilePath = Path.Combine(LocalPath, Name, "version.txt");

                if (!File.Exists(versionFilePath))
                {
                    return true;
                }

                string localVersionString = File.ReadAllText(versionFilePath).Trim();
                Version localVersion = new Version(localVersionString);

                return RemoteVersion > localVersion;
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., format issues, IO errors)
                Console.WriteLine($"Error checking update for addon at '{LocalPath}': {ex.Message}");
                return false;
            }
        }
    }
}