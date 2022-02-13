using System.IO;
using NoblegardenLauncherSharp.Globals;

namespace NoblegardenLauncherSharp.Models
{
    public class FileModel
    {
        protected static readonly string WORKING_DIR = Settings.WORKING_DIR;
        protected string PathToFile;

        public FileModel(string RelativePath) {
            PathToFile = Path.GetFullPath(
                Path.Combine(
                    WORKING_DIR,
                    RelativePath
                )
            );
        }

        protected bool Exists() {
            return File.Exists(PathToFile);
        }

        protected bool HasAnyContent() {
            return Exists() && new FileInfo(PathToFile).Length > 0;
        }
    }
}
