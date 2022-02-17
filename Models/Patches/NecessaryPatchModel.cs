using NoblegardenLauncherSharp.Interfaces;

namespace NoblegardenLauncherSharp.Models
{
    public class NecessaryPatchModel : PatchModel, IUpdateable
    {
        public NecessaryPatchModel(string LocalPath, string RemotePath, string Hash, string Description) : base(LocalPath, RemotePath, Hash, Description) {
            ChangeSelectionTo(true);
        }

        public override void ChangeSelectionTo(bool To) {
            Selected = true;
        }

        public override NecessaryPatchModel ToNecessaryPatch() {
            return this;
        }

        public override CustomPatchModel ToCustomPatchModel() {
            return new CustomPatchModel(LocalPath, RemotePath, RemoteHash, Description);
        }
    }
}