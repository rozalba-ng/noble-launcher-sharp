using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Models
{
    public class NoblePatchGroupModel<PatchType>
    {
        public List<PatchType> List;

        public NoblePatchGroupModel(IEnumerable<PatchType> Patches) {
            List = new List<PatchType>(Patches);
        }

        public PatchType GetPatchByID(int id) {
            return List[id];
        }
    }
}
