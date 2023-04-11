using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace FuelTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InteractionPage : ContentPage
    {

        private Interaction curInteraction;
        /// <summary>
        /// Krishiv Soni
        /// </summary>
        /// <param name="database"></param>
        /// <param name="custListView"></param>
        /// <param name="selectedCustomer"></param>
        public InteractionPage(Fuel_Tracker_Database_Controller database, ListView custListView, Customers selectedCustomer)
        {
            InitializeComponent();
            //Listview Code

            List<Interactions> custInteractions;
            if (database.GetAllInteractions().Count > 0)
            {
                //custInteractions = database.GetInteractionsByCustomerID(selectedCustomer.ID);
                custInteractions = database.GetAllInteractions().Where(i => i.CustomerID == selectedCustomer.ID).ToList();
            }
            else
            {
                custInteractions = new List<Interactions>();
            }
       
            ListView listView = new ListView
            {
                ItemsSource = custInteractions,
            /*    ItemTemplate = new DataTemplate(typeof(InteractionCell)),
                RowHeight = InteractionCell.RowHeight,*/
                HeightRequest = 900
            };


            //Table Code

            TableView table = new TableView { Intent = TableIntent.Form };
            EntryCell eDesc = new EntryCell { Label = "Comments" };
            ViewCell cell = new ViewCell();
            ViewCell cell2 = new ViewCell();

            DatePicker picker = new DatePicker { Format = "D" };
            List<Products> allProducts = database.GetAllProducts();

            Picker products = new Picker();

            //products.ItemsSource = allProducts;

            foreach (Products product in allProducts)
            {
                products.Items.Add(product.ToString());
            }

            cell.View = picker;
            cell2.View = products;
            SwitchCell sCompleted = new SwitchCell { Text = "Puchased?" };
            TableSection section = new TableSection("Add New Intearction for this customer") { cell, eDesc, sCompleted,  cell2};
            table.Root = new TableRoot { section };
           



            Button btnSave = new Button { Text = "Save", VerticalOptions = LayoutOptions.Start };
            btnSave.Clicked += (sender, e) =>
            {
                if (curInteraction != null)
                {
                    curInteraction.Comments = eDesc.Text;
                    curInteraction.Date = picker.Date;
                    curInteraction.Purchased = sCompleted.On;
                    curInteraction = null;
                }
                else
                {
                    Interactions item = new Interactions {   Comments = eDesc.Text, Date = picker.Date, Purchased = sCompleted.On, CustomerID= selectedCustomer.ID, ProductID = 1 };
                    custInteractions.Add(item);
                    database.SaveInteraction(item);
                    listView.ItemsSource = database.GetAllInteractions().Where(i => i.CustomerID == selectedCustomer.ID).ToList();
                }
                picker.Date = DateTime.Now;
                eDesc.Text = "";
                sCompleted.On = false;
            };
            listView.ItemTapped += (sender, e) =>
            {
                listView.SelectedItem = null;
                curInteraction = (Interaction)e.Item;
                eDesc.Text = curInteraction.Comments;
                picker.Date = curInteraction.Date;
                sCompleted.On = curInteraction.Purchased;
            };


            Content = new StackLayout
            {
                Spacing = 15,
                Padding = 25,
                Children = {

                    /*new Label { Text = "Customer page" }*/
                    listView,
                    table,
                 /*new TableView { Intent = TableIntent.Form, Root = new TableRoot { new TableSection("Add New Customer") { eFn, eLn, eAddr,ePhone, eEmail} } },*/
                    btnSave,
                }
            };

            var PageTitleToolbarItem = new ToolbarItem("Interactions Page", null, async () => { });
            PageTitleToolbarItem.Order = ToolbarItemOrder.Primary;
            ToolbarItems.Add(PageTitleToolbarItem);

            ToolbarItems.Add(new ToolbarItem("Products", null, async () =>
            {
                await Navigation.PushAsync(new ProductsPage(database, custListView));
            }));

            ToolbarItems.Add(new ToolbarItem("Settings", null, async () =>
            {
                await Navigation.PushAsync(new SettingPage(database, custListView));
            }));
        }
    }
    public class InteractionCell: ViewCell
    {
        public const int RowHeight = 85;
        private Label lblComment = new Label();
        private Label lblProduct = new Label();
        public InteractionCell()
        {
            //Comment
            lblComment.FontAttributes = FontAttributes.Bold;
            lblComment.SetBinding(Label.TextProperty, "Comments");
            //Product
            lblProduct.FontAttributes = FontAttributes.Bold;
            lblProduct.SetBinding(Label.TextProperty, "Products");


            //Date picker
            Label lblDate = new Label { FontAttributes = FontAttributes.Italic };
            lblDate.SetBinding(Label.TextProperty, "Date", stringFormat: "{0:D}");

            //Purchased Switch
            Switch swtCompleted = new Switch { HorizontalOptions = LayoutOptions.Start, IsEnabled = false };
            swtCompleted.SetBinding(Switch.IsToggledProperty, "Purchased");
            //Switch config
            StackLayout switchStack = new StackLayout { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.Start };
            switchStack.Children.Add(new Label { Text = "Purchased?" });
            switchStack.Children.Add(swtCompleted);

            Interaction t = (Interaction)this.BindingContext;

            StackLayout viewLayout = new StackLayout();
            viewLayout.Children.Add(lblDate);
            viewLayout.Children.Add(lblComment);
            viewLayout.Children.Add(switchStack);
            View = viewLayout;
        }
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            // OnBindingContextChanged is useful if there is something that you need to go and lookup (perhaps in another table in a database)
            // to display in this cell. The issue with doing this kind of work in the constructor is that when the constructor runs, 
            // the binding (link) to the object hasn't happened yet, so we can't access properties of the object in the constructor.
            // (Try accessing the Date value in the constructor, you will see that the BindingContext is null)
            Interaction item = (Interaction)this.BindingContext;
            if (item.Date < DateTime.Now) // the item is overdue
            {
                lblComment.BackgroundColor = Color.Red;
            }
            else // the item can still be completed
            {
                lblComment.BackgroundColor = Color.Green;
            }
            // This method runs after the constructor, once the "link" has been established to this row's bound Interaction object
            // Often times you may want to use a property of the bound item to do something like make a decision, so that kind of code
            // would go here.
        }
    }

    public class Interaction : INotifyPropertyChanged
    {
        private string pComment;
        private string pProduct;
        private bool pPurchased;
        private DateTime pDate;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        public string Comments
        {
            get { return this.pComment; }
            set
            {
                if (value != this.pComment)
                {
                    this.pComment = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Purchased
        {
            get { return this.pPurchased; }
            set
            {
                if (value != this.pPurchased)
                {
                    this.pPurchased = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime Date
        {
            get { return this.pDate; }
            set
            {
                if (value != this.pDate)
                {
                    this.pDate = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}