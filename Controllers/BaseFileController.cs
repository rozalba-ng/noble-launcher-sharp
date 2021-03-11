using System.IO;

namespace NoblegardenLauncherSharp.Controllers
{
    public class BaseFileController
    {
        public static readonly string WORKING_DIR = @"D:\";
        protected string PathToFile;

        public BaseFileController(string RelativePath) {
            PathToFile = Path.GetFullPath(
                Path.Combine(
                    WORKING_DIR,
                    RelativePath
                )
            );

            CreateFileIfNotExist();
            FillWithDefaults();
        }

        private void CreateFileIfNotExist() {
            if (Exists()) return;
            try {
                File.Create(PathToFile);
            } catch {
                throw new System.Exception($"Не удалось создать файл {PathToFile}");
            }
        }

        protected virtual string[] GetDefaultContent() {
            return new string[] { };
        }

        protected void FillWithDefaults() {
            if (GetDefaultContent().Length == 0 || !Exists() || HasAnyContent())
                return;

            try {
                using (StreamWriter file = new StreamWriter(PathToFile)) {
                    foreach (string s in GetDefaultContent()) {
                        file.WriteLine(s);
                    }
                }
            } catch {
                throw new System.Exception($"Не удалось заполнить файл {PathToFile} стандартными значениями");
            }
        }

        protected bool Exists() {
            return File.Exists(PathToFile);
        }

        protected bool HasAnyContent() {
            return Exists() && new FileInfo(PathToFile).Length > 0;
        }
    }
}
