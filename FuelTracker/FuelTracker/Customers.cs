using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FuelTracker
{
    public class Customers
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public override string ToString()
        {
            return FirstName + " " + LastName + "   "+ Phone;
        }
    }
}