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
    public partial class CustomerPage : ContentPage
    {


        static Fuel_Tracker_Database_Controller database;
        
        /// <summary>
        /// Krishiv SOni
        /// </summary>
        public static Fuel_Tracker_Database_Controller Database
        {
            get
            {
                if (database == null)
                {
                    database = new Fuel_Tracker_Database_Controller(DependencyService.Get<IFileHelper>().GetLocalFilePath("fuel.db3"));
                }
                return database;
            }
        }

        public CustomerPage()
        {
            InitializeComponent();
            //initialize db
            database = Database;
            NavigationPage.SetHasNavigationBar(this, true);


            //getting all the cusotmers from the database and displayign them

            List<Customers> allCustomers = database.GetAllCustomers();

            ListView listView = new ListView
            {
                ItemsSource= allCustomers,
                
            };

            listView.ItemSelected += async (s, e) =>
            {
                if(e.SelectedItem ==null)
                {
                    return;
                }
                Customers selectedCust = e.SelectedItem as Customers;

                /*if(database.GetInteractionsByCustomerID(selectedCust.ID).Count> 0)
                {
                    await Navigation.PushAsync(new InteractionPage(database, listView, selectedCust));
                }*/

                await Navigation.PushAsync(new InteractionPage(database, listView, selectedCust));


                listView.SelectedItem = null;
            };



            Button btnAddNewCust = new Button { Text = "Add New Customer" };
            btnAddNewCust.Clicked += async (s, e) =>
            {
                await Navigation.PushAsync(new AddNewCustomersPage(database, listView));
            };


            Content = new StackLayout
            {
                Spacing = 15,
                Padding = 25,
                Children = {
               
                /*new Label { Text = "Customer page" }*/
                listView,
                btnAddNewCust

                }
            };

            var  PageTitleToolbarItem = new ToolbarItem ("Customers", null, async () => { });
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