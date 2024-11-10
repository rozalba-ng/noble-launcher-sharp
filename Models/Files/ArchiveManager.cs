using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NobleLauncher
{
    internal class ArchiveManager
    {

        public static void ExtractToDirectoryWithOverwrite(string sourceArchiveFileName, string destinationDirectoryName, Encoding entryNameEncoding = null)
        {   
            using (ZipArchive archive = ZipFile.Open(Path.Combine(sourceArchiveFileName), ZipArchiveMode.Read, entryNameEncoding))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(destinationDirectoryName, entry.FullName);
                    string destinationDirectory = Path.GetDirectoryName(destinationPath);

                    if (!Directory.Exists(destinationDirectory))
                    {
                        Directory.CreateDirectory(destinationDirectory);
                    }

                    if (!string.IsNullOrEmpty(Path.GetFileName(destinationPath))) // Skip directories
                    {
                        entry.ExtractToFile(destinationPath, overwrite: true);
                    }
                }
            }
        }
    }
}
