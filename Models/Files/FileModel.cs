using System.IO;
using NobleLauncher.Globals;

namespace NobleLauncher.Models
{
    public class FileModel
    {
        protected static readonly string WORKING_DIR = Settings.WORKING_DIR;
        public string PathToFile;

        public FileModel(string RelativePath) {
            PathToFile = Path.GetFullPath(
                Path.Combine(
                    WORKING_DIR,
                    RelativePath
                )
            );
        }

        public void SetRelativePath(string RelativePath) {
            PathToFile = Path.Combine(
                    WORKING_DIR,
                    RelativePath
                );
            }

        public void Delete()
        {
            File.Delete(PathToFile);
        }
        public bool Exists() {
            return File.Exists(PathToFile);
        }

        protected bool HasAnyContent() {
            return Exists() && new FileInfo(PathToFile).Length > 0;
        }
    }
}
