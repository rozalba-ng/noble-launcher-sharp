namespace NoblegardenLauncherSharp.Models
{
    public class NoblePatchModel
    {
        public string LocalPath { get; set; }
        public string RemotePath { get; set; }
        public string Description { get; set; }
        public string Hash { get; set; }

        public NoblePatchModel() { }

        public NoblePatchModel(string LocalPath, string RemotePath, string Hash, string Description) {
            this.LocalPath = LocalPath;
            this.RemotePath = RemotePath;
            this.Hash = Hash;
            this.Description = Description;
        }
    }
}
