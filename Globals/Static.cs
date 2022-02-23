using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using NobleLauncher.Models;
using NobleLauncher.Structures;

namespace NobleLauncher.Globals
{
    public static class Static
    {
        public static DispatcherOperation CurrentUIOperation;
        public static NoblePatchGroupModel<NecessaryPatchModel> Patches;
        public static NoblePatchGroupModel<CustomPatchModel> CustomPatches;

        public static readonly List<SliderElement> SliderElements = new List<SliderElement> {
            new SliderElement("Персонажи", "https://noblegarden.net/charlist", "Images/square-character.jpg"),
            new SliderElement("Гильдии", "https://noblegarden.net/guild", "Images/square-guild.jpg"),
            new SliderElement("Сюжеты", "https://noblegarden.net/storyline", "Images/square-plots.jpg")
        };

        // Для простого биндинга в слайдер
        public static readonly BitmapImage FirstSliderElementImage = SliderElements[0].Image;
        public static readonly string FirstSliderElementLink = SliderElements[0].Link;
        public static readonly string FirstSliderElementName = SliderElements[0].Name;

        public static string GetRightWordForm(int count, string[] words) {
            if (count <= 0)
                return words[2];

            if (count < 20) {
                if (count == 1)
                    return words[0];
                if (count < 5)
                    return words[1];
                return words[2];
            }

            if (count < 100) {
                int lastNumberInCount = count % 10;
                return GetRightWordForm(lastNumberInCount, words);
            }

            int lastTenInCount = count % 100;
            return GetRightWordForm(lastTenInCount, words);
        }

        public static void OpenLinkFromTag(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            string link = target.Tag.ToString();
            Process.Start(link);
        }

        public static void InUIThread(Action action) {
            if (Application.Current == null)
                return;
            CurrentUIOperation = Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                action
            );
        }

        public static void Shutdown() {
            if (CurrentUIOperation != null) {
                CurrentUIOperation.Abort();
            }
            FileDownloader.AbortAnyLoad();

            if (Patches != null) {
                Patches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));
            }
            if (CustomPatches != null) {
                CustomPatches.List.FindAll(patch => File.Exists(patch.PathToTMP)).ForEach(patch => File.Delete(patch.PathToTMP));
            }

            if (Application.Current != null) {
                Application.Current.Shutdown();
            }
        }

        public static void ShutdownWithError(string error) {
            System.Windows.Forms.MessageBox.Show(error, "Ошибка");
            Shutdown();
        }
    }
}
