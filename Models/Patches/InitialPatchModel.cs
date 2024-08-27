using NobleLauncher.Interfaces;

namespace NobleLauncher.Models
{
    public class InitialPatchModel: PatchModel, IUpdateable
    {
        public InitialPatchModel(string LocalPath, string RemotePath, string Hash, string Description) : base(LocalPath, RemotePath, Hash, Description)
        {
            ChangeSelectionTo(true);
        }

        public override void ChangeSelectionTo(bool To)
        {
            Selected = true;
        }

        public override NecessaryPatchModel ToNecessaryPatch()
        {
            return new NecessaryPatchModel(LocalPath, RemotePath, RemoteHash, Description);
        }

        public override CustomPatchModel ToCustomPatchModel()
        {
            return new CustomPatchModel(LocalPath, RemotePath, RemoteHash, Description);
        }
    }
}