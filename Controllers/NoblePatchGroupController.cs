using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    public class NoblePatchGroupController
    {
        public ObservableCollection<NoblePatchModel> List;

        public NoblePatchGroupController(IEnumerable<NoblePatchModel> Patches) {
            List = new ObservableCollection<NoblePatchModel>(Patches);
        }

        public NoblePatchModel GetPatchByID(int id) {
            return List[id];
        }

        public List<NoblePatchModel> GetOnlySelected() {
            var SelectedPatches = new List<NoblePatchModel>();

            Parallel.For(0, List.Count, (i) => {
                if (List[i].Selected) {
                    SelectedPatches.Add(List[i]);
                }
            });

            return SelectedPatches;
        }
    }
}
