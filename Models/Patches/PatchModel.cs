using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    public class PatchModel: IUpdateable
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }
        public bool Selected { get; set; }
        private string FullPath {
            get => Settings.WORKING_DIR + "/" + LocalPath;
        }

        public PatchModel() {
            Selected = false;
        }

        public PatchModel(string LocalPath, string RemotePath, string Hash, string Description) {
            this.LocalPath = LocalPath;
            this.RemotePath = RemotePath;
            this.Hash = Hash;
            this.Description = Description;
            Selected = false;

            CalcNameFromPath();
        }

        public void CalcNameFromPath() {
            if (LocalPath == null)
                return;

            var NameParts = LocalPath.Split('/');
            Name = NameParts[NameParts.Length - 1];
        }

        public virtual void ChangeSelectionTo(bool To) {
            return;
        }

        public virtual NecessaryPatchModel ToNecessaryPatch() {
            var patch = new NecessaryPatchModel(LocalPath, RemotePath, Hash, Description) {
                Index = Index
            };
            return patch;
        }

        public virtual CustomPatchModel ToCustomPatchModel() {
            var patch = new CustomPatchModel(LocalPath, RemotePath, Hash, Description) {
                Index = Index
            };
            return patch;
        }

        public long GetPathByteSize() {
            if (!File.Exists(FullPath))
                return 0;

            return new FileInfo(FullPath).Length;
        }

        public Task<string> GetCRC32Hash(Action<long> OnBlockRead) {
            return HashCalculator.CalcCRC32Hash(FullPath, OnBlockRead);
        }
    }
}
