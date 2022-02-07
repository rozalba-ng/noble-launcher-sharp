using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для CustomPatches.xaml
    /// </summary>
    public partial class CustomPatches : UserControl
    {
        private readonly ElementSearcherController ElementSearcher;
        private readonly UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        public CustomPatches() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            Task.Run(() => GetAndDrawCustomPatches());
        }

        private void OpenCustomPatchLink(object sender, MouseButtonEventArgs e) {
            Static.OpenLinkFromTag(sender, e);
        }

        private void OnCustomPatchSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());
            var patch = Static.CustomPatches.GetPatchByID(id);
            patch.ChangeSelectionTo(!patch.Selected);
            SettingsModel.GetInstance().ToggleCustomPatchSavedSelection(patch.LocalPath);
            var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
            //var settingsScrollerView = (ScrollViewer)ElementSearcher.FindName("SettingsScrollerView");
            //var offset = settingsScrollerView.VerticalOffset;
            customPatchesView.Items.Refresh();
            //settingsScrollerView.ScrollToVerticalOffset(offset);
        }
        private async Task GetAndDrawCustomPatches() {
            var customPatchesResponse = await UpdateServerAPI.GetCustomPatches();
            var patchesInfo = customPatchesResponse.GetFormattedData();
            List<CustomPatchModel> customPatches = JObjectConverter.ConvertToCustomPatchesList(patchesInfo);
            Static.CustomPatches = new NoblePatchGroupModel<CustomPatchModel>(customPatches);

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
                    customPatchesView.ItemsSource = Static.CustomPatches.List;
                })
            );
        }
    }
}
