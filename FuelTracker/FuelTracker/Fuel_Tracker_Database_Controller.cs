using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace FuelTracker
{
    public class Fuel_Tracker_Database_Controller : ContentPage
    {
        readonly SQLiteConnection database;

        /// <summary>
        /// Krishiv SOni
        /// </summary>
        /// <param name="dbPath"></param>
        public Fuel_Tracker_Database_Controller(string dbPath)
        {
            database = new SQLiteConnection(dbPath);

            database.CreateTable<Customers>();
            database.CreateTable<Interactions>();
            database.CreateTable<Products>();

            if (database.Table<Products>().Count() == 0)
            {
                Products P1 = new Products
                {
                    Name = "Wonder Jacket",
                    Description = "A wonderfull Jacket",
                    Price= 499.99,
                };
                SaveProducts(P1);
                Products P2 = new Products
                {
                    Name = "Wonder Hat",
                    Description = "A wonderfull hat",
                    Price = 124.99,
                };
                SaveProducts(P2);
                Products P3 = new Products
                {
                    Name = "Wonder Boots",
                    Description = "A wonderfull pair of high qaulity boots",
                    Price = 224.99,
                };
                SaveProducts(P3);

            }
           

     
        }


        /// <summary>
        /// Save or update a customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>

        public int SaveCustomer(Customers customer)
        {
            if (customer.ID != 0) // existing customer
            {
                return database.Update(customer);
            }
            else // new purchase
            {
                return database.Insert(customer);
            }
        }

        /// <summary>
        /// 
        /// Same as Save Customer
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public int SaveProducts(Products product)
        {
            if (product.ID != 0) // existing customer
            {
                return database.Update(product);
            }
            else // new purchase
            {
                return database.Insert(product);
            }
        }

        /// <summary>
        /// 
        /// Same as the Same Customer 
        /// </summary>
        /// <param name="interaction"></param>
        /// <returns></returns>
        public int SaveInteraction(Interactions interaction)
        {
            if (interaction.ID != 0) // existing customer
            {
                return database.Update(interaction);
            }
            else // new purchase
            {
                return database.Insert(interaction);
            }
        }


        /// <summary>
        /// Get all the customers from the database
        /// </summary>
        /// <returns></returns>

        public List<Customers> GetAllCustomers()
        {
            return database.Table<Customers>().ToList();
        }

        /// <summary>
        /// Same as the previous method
        /// </summary>
        /// <returns></returns>
        public List<Interactions> GetAllInteractions()
        {
            return database.Table<Interactions>().ToList();
        }


        /// <summary>
        /// Same as the previous method
        /// </summary>
        /// <returns></returns>

        public List<Products> GetAllProducts()
        {
            return database.Table<Products>().ToList();
        }


        /// <summary>
        /// Reset the database
        /// </summary>

        public void resetDatabase()
        {
            database.DropTable<Customers>();
            database.DropTable<Products>();
            database.DropTable<Interactions>();

            database.CreateTable<Customers>();
            database.CreateTable<Interactions>();
            database.CreateTable<Products>();

            if (database.Table<Products>().Count() == 0)
            {
                Products P1 = new Products
                {
                    Name = "Wonder Jacket",
                    Description = "A wonderfull Jacket",
                    Price = 499.99,
                };
                SaveProducts(P1);
                Products P2 = new Products
                {
                    Name = "Wonder Hat",
                    Description = "A wonderfull hat",
                    Price = 124.99,
                };
                SaveProducts(P2);
                Products P3 = new Products
                {
                    Name = "Wonder Boots",
                    Description = "A wonderfull pair of high qaulity boots",
                    Price = 224.99,
                };
                SaveProducts(P3);

            }

        }
        
        /// <summary>
        /// Get a interactgion by Cust ID
        /// </summary>
        /// <param name="CustID"></param>
        /// <returns></returns>
        public List<Interactions> GetInteractionsByCustomerID(int CustID)
        {
            return database.Query<Interactions>("SELECT * FROM [Interactions] WHERE [CustomerID] = ?", CustID); 
        }

        /// <summary>
        /// 
        /// Same but by product ID
        /// </summary>
        /// <param name="ProID"></param>
        /// <returns></returns>
        public List<Interactions> GetInteractionsByProductID(int ProID)
        {
            return database.Query<Interactions>("SELECT * FROM [Interactions] WHERE [ProductID] = ?", ProID); 
        }


        /// <summary>
        /// Get a specific product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public Products GetProduct(int id)
        {
            return database.Table<Products>().Where(i => i.ID == id).FirstOrDefault(); // return the matching fuel purchase of the id, or null
        }


    }
}