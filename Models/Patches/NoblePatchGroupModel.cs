using System.Collections.Generic;

namespace NoblegardenLauncherSharp.Controllers
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
