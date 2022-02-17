using Force.Crc32;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Models
{
    static class HashCalculator
    {
        private static readonly int MAX_BUFFER_SIZE = 262144; //256 kb;
        public static async Task<string> CalcCRC32Hash(string path, Action<long> OnBlockRead) {
            uint hash = 0u;
            var buffer = new byte[MAX_BUFFER_SIZE];

            if (!File.Exists(path))
                return "";

            await Task.Run(() => {
                using (var f = File.OpenRead(path)) {
                    int count = 0;
                    while ((count = f.Read(buffer, 0, buffer.Length)) != 0) {
                        hash = Crc32Algorithm.Append(hash, buffer, 0, count);
                        OnBlockRead(count);
                    }
                }
            });
            return hash.ToString("X8");
        }
    }
}
