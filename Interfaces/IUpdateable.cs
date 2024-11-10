using System;
using System.Threading.Tasks;

namespace NobleLauncher.Interfaces
{
    public interface IUpdateable
    {
        string FullPath { get; }
        string LocalPath { get; set; }
        string RemotePath { get; set; }
        string PathToTMP { get; }
        string LocalHash { get; set; }
        string RemoteHash { get; set; }
        string Name { get; set; }
        bool Selected { get; set; }
        long GetPathByteSize();

        void ChangeSelectionTo(bool To);
        bool IsUpdateNeeded();
        DateTime GetLastModified();
        string CalcCRC32Hash(Action<long> OnBlockRead);
        Task<long> GetRemoteSize();
        Task<DateTime> GetRemoteLastModified();
        Task LoadUpdated(Action<long, int> OnChunkLoaded);
    }
}
