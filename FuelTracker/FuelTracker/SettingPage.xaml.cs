using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        /// <summary>
        /// Krishiv Soni
        /// </summary>
        /// <param name="database"></param>
        /// <param name="listView"></param>
        public SettingPage(Fuel_Tracker_Database_Controller database, ListView listView)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);


            Button btnReset = new Button { Text = "Reset App" };
            btnReset.Clicked += async (s, e) =>
            {
                database.resetDatabase();
                listView.ItemsSource = database.GetAllCustomers();
                await Navigation.PopAsync();
            };

            Content = new StackLayout
            {
                Spacing = 15,
                Padding = 25,
                Children = {

                new Label { Text = "Settings page" },
                btnReset

                }
            };

            var PageTitleToolbarItem = new ToolbarItem("Customers", null, async () => { });
            PageTitleToolbarItem.Order = ToolbarItemOrder.Primary;
            ToolbarItems.Add(PageTitleToolbarItem);

            ToolbarItems.Add(new ToolbarItem("Products", null, async () =>
            {
                await Navigation.PushAsync(new ProductsPage(database, listView));
            }));

            ToolbarItems.Add(new ToolbarItem("Settings", null, async () =>
            {
                await Navigation.PushAsync(new SettingPage(database, listView));
            }));
        }
    }
}