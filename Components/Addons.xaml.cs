using NobleLauncher.Globals;
using NobleLauncher.Interfaces;
using NobleLauncher.Models;
using NobleLauncher.Structures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace NobleLauncher.Components
{
    public partial class Addons : UserControl
    {
        private readonly UpdateServerAPIModel UpdateServerAPI = UpdateServerAPIModel.Instance();
        public Addons() {
            InitializeComponent();

            EventDispatcher.CreateSubscription(EventDispatcherEvent.CompletePreload, async () => {
                await Task.Run(() => GetAndDrawAddons());
                EventDispatcher.Dispatch(EventDispatcherEvent.CompleteAddonsInfoRetrieving);
            });
        }

        private void OpenAddonLink(object sender, MouseButtonEventArgs e) {
            Static.OpenLinkFromTag(sender, e);
        }

        private void OnAddonSelectorClick(object sender, RoutedEventArgs e) {
            var target = (FrameworkElement)sender;
            var id = Int32.Parse(target.Tag.ToString());
            var addon = Static.Addons.GetUpdatableById(id);
            addon.ChangeSelectionTo(!addon.Selected);
            Settings.ToggleAddonSelection(addon.Name);
            AddonsView.Items.Refresh();
            EventDispatcher.Dispatch(EventDispatcherEvent.AddonsRefresh);
        }

        private async Task GetAndDrawAddons() {
            var addonsResponse = await UpdateServerAPI.GetAddons();
            var addonsInfo = addonsResponse.FormattedData;
            List<AddonModel> addons = JObjectConverter.ConvertToAddonList(addonsInfo);
            Static.Addons = new NobleUpdatableGroupModel<IUpdateable>(addons);

            Static.InUIThread(() => {
                AddonsView.ItemsSource = Static.Addons.List;
            });
        }
    }
}
