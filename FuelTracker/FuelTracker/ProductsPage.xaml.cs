using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    
    public partial class ProductsPage : ContentPage
    {
        /// <summary>
        /// KrishivSoni
        /// </summary>
        /// <param name="database"></param>
        /// <param name="listView"></param>
        public ProductsPage(Fuel_Tracker_Database_Controller database, ListView listView)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);



            //getting all the cusotmers from the database and displayign them

            List<Products> allProducts = database.GetAllProducts();

            ListView productlistView = new ListView
            {
                ItemsSource = allProducts,
                ItemTemplate = new DataTemplate(() =>
                {
                    var name = new Label();
                    name.FontAttributes = FontAttributes.Bold;
                    name.SetBinding(Label.TextProperty, "Name");

                    var des = new Label();
                    des.SetBinding(Label.TextProperty, "Description");

                    var price = new Label();
                    price.SetBinding(Label.TextProperty, "Price", stringFormat: "{0:C}");


                    //var interactions = new Label({ Text = "Interactions: " + database.GetAllInteractions().Where(i => i.ProductID== this.BindingCon).ToList(); ;
           


                    /*Products curPro = (Products)this.BindingContext;*/


                    /*   int numOfInteractionsForCurrentProduct = 0;
                       if (database.GetInteractionsByProductID(curPro.ID) == null)
                       {
                           numOfInteractionsForCurrentProduct = database.GetInteractionsByProductID(curPro.ID).Count;
                       }*/



                    //var totalInteractions = new Label() { Text = numOfInteractionsForCurrentProduct.ToString()};

                    var layout = new StackLayout();
                    layout.Orientation = StackOrientation.Vertical;
                    layout.Children.Add(name);
                    layout.Children.Add(des);
                    layout.Children.Add(price);
                    //layout.Children.Add(totalInteractions);


                    return new ViewCell { View = layout };

                }),
                RowHeight= 100,
                HeightRequest = 900

            };

            Content = new StackLayout
            {
                Spacing = 15,
                Padding = 25,
                Children = {

                /*new Label { Text = "Products page" }*/
                productlistView

                }
            };

            var PageTitleToolbarItem = new ToolbarItem("Products Page", null, async () => { });
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