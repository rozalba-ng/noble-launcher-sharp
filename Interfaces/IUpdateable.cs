using System;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Interfaces
{
    interface IUpdateable
    {
        string FullPath { get; }
        string LocalPath { get; set; }
        long GetPathByteSize();
        Task<string> GetCRC32Hash(Action<long> OnBlockRead);
    }
}
