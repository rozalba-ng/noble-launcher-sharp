using System.Collections.Generic;
using System.Threading.Tasks;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class NoblePatchGroupController
    {
        public List<NoblePatchModel> Patches = new List<NoblePatchModel>();

        public NoblePatchGroupController(IEnumerable<NoblePatchModel> Patches) {
            this.Patches.AddRange(Patches);
        }

        public NoblePatchModel GetPatchByID(int id) {
            return Patches[id];
        }

        public List<NoblePatchModel> GetOnlySelected() {
            var SelectedPatches = new List<NoblePatchModel>();

            Parallel.For(0, Patches.Count, (i) => {
                if (Patches[i].Selected) {
                    SelectedPatches.Add(Patches[i]);
                }
            });

            return SelectedPatches;
        }
    }
}
