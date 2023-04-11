using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using static Xamarin.Essentials.Permissions;

namespace FuelTracker
{
    public class Interactions
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public int CustomerID{ get; set; }
        public DateTime Date { get; set; }
        public string Comments { get; set; }
        public int ProductID { get; set; }
        public bool Purchased { get; set; }

        public override string ToString()
        {
            return "Comments: "+ Comments + " Purchased? " + Purchased+ " Date: " + Date;
        }
    }
}