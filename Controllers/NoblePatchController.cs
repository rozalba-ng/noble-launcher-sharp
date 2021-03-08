using NoblegardenLauncherSharp.Models;

namespace NoblegardenLauncherSharp.Controllers
{
    class NoblePatchController
    {
        private readonly NoblePatchModel Patch;

        public NoblePatchController(NoblePatchModel Patch) {
            this.Patch = Patch;
        }

        public void ToggleSelection() {
            Patch.Selected = !Patch.Selected;
        }
    }
}
