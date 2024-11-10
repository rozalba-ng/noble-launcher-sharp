using System.Collections.Generic;

namespace NobleLauncher.Models
{
    public class NobleUpdatableGroupModel<UpdatableType>
    {
        public List<UpdatableType> List;

        public NobleUpdatableGroupModel(IEnumerable<UpdatableType> Updatables) {
            List = new List<UpdatableType>(Updatables);
        }

        public UpdatableType GetUpdatableById(int id) {
            return List[id];
        }

        public int Count() { return List.Count; }
    }
}
