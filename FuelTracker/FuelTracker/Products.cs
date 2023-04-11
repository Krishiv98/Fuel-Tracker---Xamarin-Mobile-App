using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace FuelTracker
{
    public class Products
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return Name + " - " + Description+ ", Price:" + Price;
        }
    }
}