namespace NoblegardenLauncherSharp.Models
{
    public class NoblePatchModel
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }
        public bool Selected { get; set; }

        public NoblePatchModel() {
            Selected = false;
        }

        public NoblePatchModel(string LocalPath, string RemotePath, string Hash, string Description) {
            this.LocalPath = LocalPath;
            this.RemotePath = RemotePath;
            this.Hash = Hash;
            this.Description = Description;
            Selected = false;
        }

        public void CalcNameFromPath() {
            if (LocalPath == null)
                return;

            var NameParts = LocalPath.Split('/');
            Name = NameParts[NameParts.Length - 1];
        }

        public void ToggleSelection() {
            Selected = !Selected;
        }
    }
}
