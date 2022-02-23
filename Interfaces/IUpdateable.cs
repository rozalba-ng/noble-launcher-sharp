using System;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Interfaces
{
    interface IUpdateable
    {
        string FullPath { get; }
        string LocalPath { get; set; }
        string RemotePath { get; set; }
        string PathToTMP { get; }
        string LocalHash { get; set; }
        string RemoteHash { get; set; }
        long GetPathByteSize();
        Task<string> CalcCRC32Hash(Action<long> OnBlockRead);
        Task<long> GetRemoteSize();
        Task LoadUpdated(Action<long, int> OnChunkLoaded);
    }
}
