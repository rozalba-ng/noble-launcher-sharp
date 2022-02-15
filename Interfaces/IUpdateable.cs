using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoblegardenLauncherSharp.Interfaces
{
    interface IUpdateable
    {
        long GetPathByteSize();
        Task<string> GetCRC32Hash(Action<long> OnBlockRead);
    }
}
