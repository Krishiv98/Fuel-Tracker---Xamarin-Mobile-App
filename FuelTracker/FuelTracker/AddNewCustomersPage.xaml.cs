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
    public partial class AddNewCustomersPage : ContentPage
    {
     /// <summary>
     /// Krishiv Soni
     /// </summary>
     /// <param name="database"></param>
     /// <param name="listView"></param>
        public AddNewCustomersPage(Fuel_Tracker_Database_Controller database, ListView listView)
        {
            InitializeComponent();


            EntryCell eFn = new EntryCell { Label = "First Name:" };
            EntryCell eLn = new EntryCell { Label = "Last Name:" };
            EntryCell eAddr = new EntryCell { Label = "Address:" };
            EntryCell ePhone = new EntryCell { Label = "Phone:" };
            EntryCell eEmail = new EntryCell { Label = "Email:" };

            Button btnSave = new Button { Text = "Save" };
            btnSave.Clicked += async(s, e) =>
            {
                Customers customer = new Customers
                {
                    FirstName = eFn.Text,
                    LastName = eLn.Text,
                    Email = eAddr.Text,
                    Address = eAddr.Text,
                    Phone = Convert.ToInt32(ePhone.Text)
                };

                eFn.Text = "";
                eLn.Text = "";
                eAddr.Text = "";
                eEmail.Text = "";
                ePhone.Text = "";
                database.SaveCustomer(customer);
                listView.ItemsSource = database.GetAllCustomers();
                await Navigation.PopAsync();

            };


            Content = new StackLayout
            {
                Spacing = 15,
                Padding = 25,
                Children = {

                    /*new Label { Text = "Customer page" }*/

                 new TableView { Intent = TableIntent.Form, Root = new TableRoot { new TableSection("Add New Customer") { eFn, eLn, eAddr,ePhone, eEmail} } },
                    btnSave,

                }
            };

            var PageTitleToolbarItem = new ToolbarItem("New Customers Page", null, async () => { });
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