using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new CustomerPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
