﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                File.Create(PathToFile).Close();
            } catch {
                throw new Exception($"Не удалось создать файл {PathToFile}");
            }
        }

        protected virtual Dictionary<string, string> GetDefaultContent() {
            return new Dictionary<string, string>();
        }

        protected void FillWithDefaults() {
            if (GetDefaultContent().Count == 0 || !Exists() || HasAnyContent())
                return;

            try {
                using (Stream file = new FileStream(PathToFile, FileMode.Open, FileAccess.Write, FileShare.Read)) {
                    using (StreamWriter writer = new StreamWriter(file)) {
                        var defaultContent = GetDefaultContent();
                        foreach (KeyValuePair<string, string> entry in GetDefaultContent()) {
                            writer.WriteLine($"{entry.Key}={entry.Value}");
                        }
                    }
                }
            } catch {
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

        protected bool Exists() {
            return File.Exists(PathToFile);
        }

        protected bool HasAnyContent() {
            return Exists() && new FileInfo(PathToFile).Length > 0;
        }
    }
}