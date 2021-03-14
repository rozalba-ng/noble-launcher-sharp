using System;
using System.Collections.Generic;
using System.IO;

namespace NoblegardenLauncherSharp.Models
{
    public class FileWithDefaultDictionaryContentModel : FileModel
    {
        public Dictionary<string, string> DefaultContent;
        public FileWithDefaultDictionaryContentModel(string RelativePath, Dictionary<string, string> DefaultContent) : base(RelativePath) {
            this.DefaultContent = DefaultContent;
            CreateFileIfNotExist();
            FillWithDefaults();
        }

        private void CreateFileIfNotExist() {
            if (Exists())
                return;
            try {
                File.Create(PathToFile).Close();
            }
            catch {
                throw new Exception($"Не удалось создать файл {PathToFile}");
            }
        }

        protected void FillWithDefaults() {
            if (DefaultContent.Count == 0 || !Exists() || HasAnyContent())
                return;

            try {
                using (Stream file = new FileStream(PathToFile, FileMode.Open, FileAccess.Write, FileShare.Read)) {
                    using (StreamWriter writer = new StreamWriter(file)) {
                        foreach (KeyValuePair<string, string> entry in DefaultContent) {
                            writer.WriteLine($"{entry.Key}={entry.Value}");
                        }
                    }
                }
            }
            catch {
                throw new Exception($"Не удалось заполнить файл {PathToFile} стандартными значениями");
            }
        }

        protected Dictionary<string, string> ReadAsDictionary() {
            var content = new Dictionary<string, string>();
            if (!Exists() || !HasAnyContent())
                return content;

            try {
                using (Stream file = new FileStream(PathToFile, FileMode.Open, FileAccess.Read, FileShare.None)) {
                    using (StreamReader reader = new StreamReader(file)) {
                        string line;
                        while ((line = reader.ReadLine()) != null) {
                            var lineParts = line.Split('=');
                            if (lineParts.Length < 2)
                                throw new Exception();

                            content.Add(lineParts[0], lineParts[1]);
                        }
                    }
                }
            }
            catch {
                throw new Exception($"Не удалось прочитать файл {PathToFile} как экземпляр Dictionary");
            }

            return content;
        }
    }
}
