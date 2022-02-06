using NoblegardenLauncherSharp.Controllers;
using NoblegardenLauncherSharp.Models;
using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace NoblegardenLauncherSharp.Components
{
    /// <summary>
    /// Логика взаимодействия для CustomPatches.xaml
    /// </summary>
    public partial class CustomPatches : UserControl
    {
        private readonly SettingsBlockController SettingsBlockController;
        private readonly ElementSearcherController ElementSearcher;
        private readonly UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        public CustomPatches() {
            InitializeComponent();
            ElementSearcher = new ElementSearcherController(this);
            SettingsBlockController = SettingsBlockController.Init();
            Task.Run(() => GetAndDrawCustomPatches());
        }

        private void OpenCustomPatchLink(object sender, MouseButtonEventArgs e) {
            Globals.OpenLinkFromTag(sender, e);
        }

        private void OnCustomPatchSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());
            SettingsBlockController.ToggleCustomPatchSelection(id);
        }
        private async Task GetAndDrawCustomPatches() {
            var customPatchesResponse = await UpdateServerAPI.GetCustomPatches();
            var patchesInfo = customPatchesResponse.GetFormattedData();
            List<CustomPatchModel> customPatches = JObjectConverter.ConvertToCustomPatchesList(patchesInfo);
            Globals.CustomPatches = new NoblePatchGroupModel<CustomPatchModel>(customPatches);

            await Application.Current.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() => {
                    var customPatchesView = (ListView)ElementSearcher.FindName("CustomPatchesView");
                    customPatchesView.ItemsSource = Globals.CustomPatches.List;
                })
            );
        }
    }
}
