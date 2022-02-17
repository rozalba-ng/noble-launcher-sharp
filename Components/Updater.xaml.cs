using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Interfaces;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для Updater.xaml
    /// </summary>
    public partial class Updater : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        private readonly ProgressBar ProgressBar;
        private readonly TextBlock CurrentAction;
        private readonly TextBlock CurrentProgress;

        public Updater() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
            ProgressBar = (ProgressBar)ElementSearcher.FindName("Progress");
            CurrentAction = (TextBlock)ElementSearcher.FindName("CurrentActionText");
            CurrentProgress = (TextBlock)ElementSearcher.FindName("CurrentActionProgress");
            EventDispatcher.CreateSubscription(EventDispatcherEvent.StartUpdate, Update);
        }
        public async void Update() {
            List<IUpdateable> patches = Static.Patches.List.ToList<IUpdateable>();
            await CalcHashes(patches);
        }

        private long GetSummaryHashFileSize(List<IUpdateable> patches) {
            long summaryFileSize = 0;
            for (int i = 0; i < patches.Count; i++) {
                summaryFileSize += patches[i].GetPathByteSize();
            };

            return summaryFileSize;
        }

        private Task CalcHashes(List<IUpdateable> patches) {
            long currentRead = 0;
            long summarySize = GetSummaryHashFileSize(patches);

            return Task.Run(async () => {
                for (int i = 0; i < patches.Count; i++) {
                    var patch = patches[i];
                    Static.ChangeUI(() => {
                        CurrentAction.Text = "Считаем чек-суммы: " + patch.LocalPath;
                    });
                    await patch.GetCRC32Hash((blockSize) => {
                        currentRead += blockSize;
                        double readBytesProgress = (double)(currentRead) / (double)(summarySize);
                        int progress = (int)(readBytesProgress * 100);
                        Static.ChangeUI(() => {
                            if (progress != ProgressBar.Value) {
                                ProgressBar.Value = progress;
                                CurrentProgress.Text = progress.ToString() + "%";
                            }
                        });
                    });
                }
            });
        }
    }
}
