using NoblegardenLauncherSharp.Globals;
using NoblegardenLauncherSharp.Models;
using NoblegardenLauncherSharp.Structures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace NoblegardenLauncherSharp.Components
{
    public partial class CustomPatches : UserControl
    {
        private readonly ElementSearcher ElementSearcher;
        private readonly UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        public CustomPatches() {
            InitializeComponent();
            ElementSearcher = new ElementSearcher(this);
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
            Settings.ToggleCustomPatchSelection(patch.LocalPath);
            var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
            customPatchesView.Items.Refresh();
            EventDispatcher.Dispatch(EventDispatcherEvent.SettingsRefresh);
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
