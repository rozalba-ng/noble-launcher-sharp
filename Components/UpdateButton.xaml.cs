using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Interfaces;
using NoblegardenLauncherSharp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для UpdateButton.xaml
    /// </summary>
    public partial class UpdateButton : UserControl
    {
        long CurrentRead = 0;
        long SummaryFileSize = 0;
        public UpdateButton() {
            InitializeComponent();
        }

        public void StartUpdate(object sender, MouseButtonEventArgs e) {
            List<IUpdateable> patches = Static.Patches.List.ToList<IUpdateable>();

            CalcSummaryFileSize(patches);
            CalcHashes(patches);
        }

        private void CalcSummaryFileSize(List<IUpdateable> patches) {
            for(int i = 0; i < patches.Count; i++) {
                SummaryFileSize += patches[i].GetPathByteSize();
            };
        }

        private void CalcHashes(List<IUpdateable> patches) {
            for (int i = 0; i < patches.Count; i++) {
                var patch = patches[i];
                SummaryFileSize += patch.GetPathByteSize();
                patch.GetCRC32Hash((blockSize) => {
                    CurrentRead += blockSize;
                    double progress = (double)(CurrentRead) / (double)(SummaryFileSize);
                    Console.WriteLine(CurrentRead + ", " + SummaryFileSize + ", " + progress + "%");
                });
            };
        }
    }
}
